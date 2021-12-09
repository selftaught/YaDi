using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YADI.Externals;


namespace YADI.Injection
{
    class QueueUserAPC
    {
        private int pid;

        public QueueUserAPC(int pid)
        {
            this.pid = pid;
        }

        public bool Inject(String dllPath)
        {
            IntPtr procHandle = Kernel32.OpenProcess(
                Kernel32.PROCESS_CREATE_THREAD |
                Kernel32.PROCESS_QUERY_INFORMATION |
                Kernel32.PROCESS_VM_OPERATION |
                Kernel32.PROCESS_VM_WRITE |
                Kernel32.PROCESS_VM_READ,
                false, (uint)this.pid);


            if (procHandle == null)
            {
                MessageBox.Show("Couldn't get process handle...");
                return false;
            }

            // Read DLL into a byte array so we know how much memory to VA.
            byte[] dllBytes = File.ReadAllBytes(dllPath);

            // Allocate memory for DLL bytes
            IntPtr writeAddr = Kernel32.VirtualAllocEx(procHandle, IntPtr.Zero, (uint)dllBytes.Length, Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT, Kernel32.PAGE_READWRITE);

            // Write DLL to memory
            UIntPtr bytesWritten = UIntPtr.Zero;

            if (!Kernel32.WriteProcessMemory(procHandle, writeAddr, Encoding.ASCII.GetBytes(dllPath), (uint)dllBytes.Length, out bytesWritten))
            {
                MessageBox.Show("Failed to write DLL path to process memory...");
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            // Iterate through process' threads and attempt to queueuserapc on it each
            Process process = Process.GetProcessById(this.pid);

            for (int i = 0; i < process.Threads.Count; i++)
            {
                ProcessThread pt = process.Threads[i];
#if DEBUG
                Console.WriteLine("Checking thread " + i + " of " + process.Threads.Count);
                Console.WriteLine("\tThread ID: " + pt.Id);
#endif
                // Get a thread handle that we can pass in the following Kernel32 calls
                IntPtr threadHandle = Kernel32.OpenThread(Kernel32.ThreadAccess.THREAD_HIJACK, false, (uint)pt.Id);

                // Handle the case where we don't get a handle back
                if (threadHandle == null || threadHandle == IntPtr.Zero)
                {
#if DEBUG
                    Console.WriteLine("Couldn't open handle to thread (ID: " + pt.Id + ")");
#endif
                    // Try the next thread
                    continue;
                }
            }

            process.Close();

            // Free our VA'd memory
            if (!Kernel32.VirtualFreeEx(procHandle, writeAddr, 0, Kernel32.MEM_RELEASE))
            {
#if DEBUG
                Console.WriteLine("VirtualFreeEx() failed!");
#endif
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            // Close our handle to the process
            Kernel32.CloseHandle(procHandle);

            return true;
        }
    }
}
