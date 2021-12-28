using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;


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
                    throw new Exception("Couldn't OpenProcess (PID: " + Pid + ")");
                }

                List<Structs.Module> modules = Helpers.Process.GetProcessModules(Pid);
                
                Structs.Module module = null;
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(Pid);
                String procFilename = Helpers.Process.GetFilename(p);

                if (procFilename != null)
                {
                    Console.WriteLine(procFilename);
                }

                foreach (Structs.Module m in modules)
                {
                    if (m.Path == procFilename)
                    {
                        module = m;
                        break;
                    }
                }


                //IntPtr hMapObject = Externals.Kernel32.CreateFileMapping();

                Console.WriteLine("ImageSize:  " + module.ImageSize.ToString());
                Console.WriteLine("BaseAddr:   " + module.BaseAddr.ToString());
                Console.WriteLine("EntryPoint: " + module.EntryPoint.ToString());
            }
        }
    }
}
