using System;
using System.IO;
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
            FileSize = (File.Exists(Path) ? new System.IO.FileInfo(Path).Length : 0);
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

        public Structs.IMAGE_DOS_HEADER GetImageDosHeader(String szFilePath)
        {
            if (szFilePath.Length == 0)
            {
                throw new Exception("Path to PE undefined!");
            }

            Structs.IMAGE_DOS_HEADER sImageDosHeader = new Structs.IMAGE_DOS_HEADER();

            IntPtr hMapObject = IntPtr.Zero;
            IntPtr hFileMapView = IntPtr.Zero;

            using (FileStream fs = File.OpenRead(szFilePath))
            {
                hMapObject = Externals.Kernel32.CreateFileMapping(fs.SafeFileHandle.DangerousGetHandle(), IntPtr.Zero, Enums.AllocationProtect.PAGE_READONLY, 0, 0, null);
                hFileMapView = Externals.Kernel32.MapViewOfFile(hMapObject, Enums.FileMapAccess.FileMapRead, 0, 0, UIntPtr.Zero);
                sImageDosHeader = (Structs.IMAGE_DOS_HEADER)Marshal.PtrToStructure(hFileMapView, typeof(Structs.IMAGE_DOS_HEADER));

                if (sImageDosHeader.e_magic[0] != 0x4D || sImageDosHeader.e_magic[1] != 0x5A)
                {
                    throw new Exception(szFilePath + " is not a valid PE file!");
                }
            }

            return sImageDosHeader;
        }

        public Structs.IMAGE_NT_HEADERS32 GetImageNtHeaders32()
        {
            Structs.IMAGE_NT_HEADERS32 sImageNtHeaders32 = new Structs.IMAGE_NT_HEADERS32();

            return sImageNtHeaders32;
        }

        public Structs.IMAGE_NT_HEADERS64 GetImageNtHeaders64()
        {
            Structs.IMAGE_NT_HEADERS64 sImageNtHeaders64 = new Structs.IMAGE_NT_HEADERS64();

            return sImageNtHeaders64;
        }

        private void ParseFromFile()
        {
            if (FilePath.Length == 0)
            {
                throw new Exception("Path to PE undefined!");
            }

            if (!File.Exists(FilePath))
            {
                throw new Exception("Path to PE doesn't exist!");
            }
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

                if (procFilename == String.Empty)
                {
                    MessageBox.Show("Couldn't get filename of process (ID: " + Pid + ")!", "Error");
                    Externals.Kernel32.CloseHandle(hProc);
                    return;
                }

                foreach (Structs.Module m in modules)
                {
                    if (m.Path == procFilename || m.Path.IndexOf(procFilename, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        module = m;
                        break;
                    }
                }

                if (module == null)
                {
                    MessageBox.Show("Couldn't find process' MainModule!", "ERROR");
                    return;
                }
                else if (File.Exists(module.Path))
                {
                    Structs.IMAGE_DOS_HEADER sImageDosHeader = GetImageDosHeader(module.Path);
                    Structs.IMAGE_NT_HEADERS32 sImageNtHeaders32;
                    Structs.IMAGE_NT_HEADERS64 sImageNtHeaders64;

                    // If the host OS isn't 64 bit, we can gaurantee
                    // the target PE is 32 bit
                    if (!Environment.Is64BitOperatingSystem)
                    {
                        Console.WriteLine("Getting x86 NT Headers from PE");
                        sImageNtHeaders32 = GetImageNtHeaders32();
                    }
                    else
                    {
                        // OS is 64 bit so we can't be sure if the target PE
                        // is 32 bit or 64 bit without a call is IsWow64Process..
                        if (Helpers.Process.IsWow64Process(p))
                        {

                        }
                    }
                }
            }
        }
    }
}
