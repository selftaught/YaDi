using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using YADI.Externals;

namespace YADI.Injection
{
    class ThreadHijack : Injectors.Base
    {
        private int pid;

        public ThreadHijack(int pid)
        {
            this.pid = pid;
        }

        public bool Inject(String dllPath)
        {
            Process process = Process.GetProcessById((int)this.pid);

            if (process == null)
            {
#if DEBUG
                Console.WriteLine("Couldn't get process by ID...");
#endif
                return false;
            }

            ProcessThread processThread = process.Threads[0];

#if DEBUG
            Console.WriteLine("Process thread id acquired: " + processThread.Id.ToString());
#endif

            IntPtr pOpenThread = Kernel32.OpenThread(Kernel32.ThreadAccess.THREAD_HIJACK, false, (uint)processThread.Id);

            if (pOpenThread == null)
            {
                MessageBox.Show("OpenThread() returned null...");
                return false;
            }

            int pSuspendedThreadStatus = Kernel32.SuspendThread(pOpenThread);
                
            if (pSuspendedThreadStatus == -1)
            {
#if DEBUG
                Console.Write("Couldn't suspend thread...");
#endif
                return false;
            }

            Structs.Context64 ctx64 = new Structs.Context64();

            ctx64.ContextFlags = Enums.ContextFlags.CONTEXT_FULL;

            if (!Kernel32.GetThreadContext(pOpenThread, ref ctx64))

            {
                Console.WriteLine("Couldn't get thread context...");
                Kernel32.ResumeThread(pOpenThread);
                return false;
            }

            byte[] dllBytes = File.ReadAllBytes(dllPath);

#if DEBUG
            Console.WriteLine("Thread RIP: " + ctx64.Rip.ToString());
            Console.WriteLine(dllBytes.ToString());
#endif

            IntPtr procHandle = Kernel32.OpenProcess(Kernel32.PROCESS_ALL_ACCESS, false, (uint)this.pid);

            if (procHandle == null)
            {
#if DEBUG
                Console.WriteLine("OpenProcess() failed");
#endif
                Kernel32.ResumeThread(pOpenThread);
                return false;
            }

            uint allocType = Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT;
            uint protect = Kernel32.PAGE_READWRITE;

            IntPtr dllBytesWriteAddr = Kernel32.VirtualAllocEx(procHandle, IntPtr.Zero, ((uint)dllBytes.Length), allocType, protect);

            if (dllBytesWriteAddr == null || dllBytesWriteAddr == IntPtr.Zero)
            {
#if DEBUG
                Console.WriteLine("Couldn't allocate memory for writing DLL bytes...");
#endif
                Kernel32.ResumeThread(pOpenThread);
                return false;
            }

            UIntPtr bytesWritten = UIntPtr.Zero;

            if (!Kernel32.WriteProcessMemory(procHandle, dllBytesWriteAddr, dllBytes, (uint)dllBytes.Length, out bytesWritten))
            {
#if DEBUG
                Console.WriteLine("Couldn't write dll path to target process memory");
#endif
                Kernel32.ResumeThread(pOpenThread);
                return false;
            }

            ctx64.Rip = (ulong)dllBytesWriteAddr;

#if DEBUG
            Console.WriteLine("Wrote " + bytesWritten.ToString() + " bytes to process at address: " + dllBytesWriteAddr.ToString());
            Console.WriteLine("Redirecting thread execution to: " + ctx64.Rip.ToString());
#endif

            if (!Kernel32.SetThreadContext(pOpenThread, ref ctx64))
            {
#if DEBUG
                Console.WriteLine("SetThreadContext() returned false..");
#endif
                MessageBox.Show("ERROR: Couldn't set thread context!");
                return false;
            }

#if DEBUG
            Console.WriteLine("Thread context with new RIP set successfully");
#endif

            Kernel32.ResumeThread(pOpenThread);
            Kernel32.CloseHandle(pOpenThread);

            return true;
        }
    }
}
