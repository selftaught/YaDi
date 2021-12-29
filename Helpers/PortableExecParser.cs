using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;


// https://pastebin.com/LYgVEd5u

namespace YADI.Helpers
{
    class PortableExecParser
    {
        private int Pid;
        private String FilePath;
        private long FileSize;

        public PortableExecParser(String Path)
        {
            Pid = -1;
            FilePath = Path;
            FileSize = new System.IO.FileInfo(Path).Length;
        }

        public PortableExecParser(int PID)
        {
            Pid = PID;
            FilePath = String.Empty;
        }

        public void Parse()
        {
            if (Pid > 0)
            {
                ParseFromProcessMemory();
            }
            else if (FilePath != null && FilePath.Length > 0)
            {
                ParseFromFile();
            }
            else
            {
                throw new Exception("PID and FilePath not provided!");
            }
        }

        private void ParseFromFile()
        {
            Console.WriteLine("Parsing PE from exec file: " + FilePath);

            if (FilePath.Length == 0)
            {
                throw new Exception("Path to PE undefined!");
            }

            Console.WriteLine("File length: " + FileSize);
        }

        private void ParseFromProcessMemory()
        {
            Console.WriteLine("Parsing PE from process memory");

            if (Pid > 0)
            {
                IntPtr hProc = Externals.Kernel32.OpenProcess(
                    Externals.Kernel32.PROCESS_CREATE_THREAD |
                    Externals.Kernel32.PROCESS_QUERY_INFORMATION |
                    Externals.Kernel32.PROCESS_VM_OPERATION |
                    Externals.Kernel32.PROCESS_VM_WRITE |
                    Externals.Kernel32.PROCESS_VM_READ,
                    false, (uint)Pid
                );

                if (hProc == IntPtr.Zero)
                {
                    Console.WriteLine("Couldn't OpenProcess (PID: " + Pid + ")");
                    return;
                }

                List<Structs.Module> modules = Helpers.Process.GetProcessModules(Pid);
                
                Structs.Module module = null;
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(Pid);
                String procFilename = Helpers.Process.GetFilename(p);

                if (procFilename == null || procFilename == String.Empty)
                {
                    MessageBox.Show("Couldn't get filename of process (ID: " + Pid + ")!", "Error");
                    return;
                }
                else
                {
                    Console.WriteLine("Searching for '" + procFilename + "' in module list...");
                }

                uint index = 0;
                foreach (Structs.Module m in modules)
                {
#if DEBUG
                    Console.WriteLine("module[" + index + "].Path = " + m.Path);
#endif
                    if (m.Path == procFilename || m.Path.IndexOf(procFilename, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        module = m;
                        break;
                    }

                    index++;
                }

                if (module == null)
                {
                    MessageBox.Show("Couldn't find process' MainModule!", "ERROR");
                    return;
                }

                if (module != null && File.Exists(module.Path))
                {
                    IntPtr hMapObject = IntPtr.Zero;
                    IntPtr hFileMapView = IntPtr.Zero;

                    Structs.IMAGE_DOS_HEADER sImageDosHeader;
                    Structs.IMAGE_NT_HEADERS32 sImageNtHeaders;

                    using (FileStream fs = File.OpenRead(module.Path))
                    {
                        hMapObject = Externals.Kernel32.CreateFileMapping(fs.SafeFileHandle.DangerousGetHandle(), IntPtr.Zero, Enums.AllocationProtect.PAGE_READONLY, 0, 0, null);
                        hFileMapView = Externals.Kernel32.MapViewOfFile(hMapObject, Enums.FileMapAccess.FileMapRead, 0, 0, UIntPtr.Zero);
                        sImageDosHeader = (Structs.IMAGE_DOS_HEADER)Marshal.PtrToStructure(hFileMapView, typeof(Structs.IMAGE_DOS_HEADER));

                        if (sImageDosHeader.e_magic[0] != 0x5A ||
                            sImageDosHeader.e_magic[1] != 0x4D)
                        {
                            throw new Exception(module.Path + " is not a valid PE file!");
                        }

                        Console.WriteLine("Pages in file: " + sImageDosHeader.e_cp);
                    }

                    Console.WriteLine("ImageSize:  " + module.ImageSize.ToString());
                    Console.WriteLine("BaseAddr:   " + module.BaseAddr.ToString());
                    Console.WriteLine("EntryPoint: " + module.EntryPoint.ToString());
                }
            }
        }
    }
}
