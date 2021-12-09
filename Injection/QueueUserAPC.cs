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

            if (writeAddr == IntPtr.Zero ||  writeAddr == null)
            {
                MessageBox.Show("VirtualAllocEx() was unsuccessful");
                return false;
            }

            // Write DLL to memory
            UIntPtr bytesWritten = UIntPtr.Zero;

            if (!Kernel32.WriteProcessMemory(procHandle, writeAddr, dllBytes, (uint)dllBytes.Length, out bytesWritten))
            {
                MessageBox.Show("Failed to write DLL path to process memory...");
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            Process process = Process.GetProcessById(this.pid);
            bool queued = false;

            // Iterate through process' threads and attempt to queueuserapc on each
            // until we find one.
            for (int i = 0; i < process.Threads.Count; i++)
            {
                int threadId = process.Threads[i].Id;
#if DEBUG
                Console.WriteLine("Checking thread " + i + " of " + (process.Threads.Count - 1));
                Console.WriteLine("\tThread ID: " + threadId);
#endif
                // Get a thread handle that we can pass in the following Kernel32 calls
                IntPtr threadHandle = Kernel32.OpenThread(Kernel32.ThreadAccess.THREAD_HIJACK, false, (uint)threadId);

                // If we don't get a handle back on the thread
                if (threadHandle == null || threadHandle == IntPtr.Zero)
                {
#if DEBUG
                    Console.WriteLine("Couldn't open handle to thread (ID: " + threadId + ")");
#endif
                    // Try the next thread
                    continue;
                }

#if DEBUG
                Console.WriteLine("Got thread handle: " + threadHandle + "\nAttempting to suspend thread");
#endif

                // Suspend the thread so we can QueueUserAPC
                int pSuspendedThreadStatus = Kernel32.SuspendThread(threadHandle);

                if (pSuspendedThreadStatus == -1)
                {
#if DEBUG
                    Console.Write("SuspendThread failed (thread id: " + threadId + ")...");
#endif
                    // Close the handle on the current thread
                    Kernel32.CloseHandle(threadHandle);

                    // Try the next thread
                    continue;
                }

                // QueueUserAPC
                if (Kernel32.QueueUserAPC(writeAddr, threadHandle, UIntPtr.Zero) != 0)
                {
#if DEBUG
                    Console.WriteLine("QueueUserAPC was unsuccessful");
#endif
                    // ResumeThread
                    Kernel32.ResumeThread(threadHandle);

                    // Close thread handle
                    Kernel32.CloseHandle(threadHandle);

                    continue;
                }

                queued = true;

                // ResumeThread
                Kernel32.ResumeThread(threadHandle);

                // Close thread handle
                Kernel32.CloseHandle(threadHandle);
            }

            if (!queued)
            {
                MessageBox.Show("QueueUserAPC injection failed");
            }

            // Free our VA'd memory
            if (!Kernel32.VirtualFreeEx(procHandle, writeAddr, 0, Kernel32.MEM_RELEASE))
            {
#if DEBUG
                Console.WriteLine("VirtualFreeEx() failed!");
#endif
                // Close process handle before returning
                Kernel32.CloseHandle(procHandle);

                return false;
            }

#if DEBUG
            Console.WriteLine("Successfully free'd VA memory");
#endif

            // Close process handle before returning
            Kernel32.CloseHandle(procHandle);
            process.Close();

            return true;
        }
    }
}
