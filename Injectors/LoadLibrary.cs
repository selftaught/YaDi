using System;
using System.Text;
using System.Windows.Forms;

using YADI.Externals;

namespace YADI.Injection
{
    public class LoadLibrary : Injectors.Base
    {
        private int pid;
        public LoadLibrary(int _pid)
        {
            pid = _pid;
        }

        public override bool Inject(String dllPath)
        {
            if (pid < 0)
            {
                MessageBox.Show("Invalid PID...");
                return false;
            }

            IntPtr procHandle = Kernel32.OpenProcess(Externals.Kernel32.PROCESS_INJECT, false, (uint)pid);
            IntPtr LoadLibraryAddr = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (procHandle == null)
            {
                MessageBox.Show("Couldn't get process handle...");
                return false;
            }

            if (Environment.Is64BitOperatingSystem)
            {
                if (Helpers.Process.Is32Bit(pid) && Helpers.DLL.Is64Bit(dllPath))
                {
                    MessageBox.Show("Can't inject a 64 bit DLL into a 32 bit process!");
                    return false;
                }
            }

#if DEBUG
            Console.WriteLine("Acquired process handle: " + procHandle.ToString("x8"));
#endif

            uint dllPathLen = (uint)dllPath.Length;

            IntPtr dllPathBaseAddr = Kernel32.VirtualAllocEx(procHandle, IntPtr.Zero, dllPathLen, Kernel32.MEM_RESERVE | Kernel32.MEM_COMMIT, Kernel32.PAGE_READWRITE);
            String dllPathBaseAddrHexStr = "0x" + dllPathBaseAddr.ToString("x8");

            if (dllPathBaseAddr == null)
            {
                MessageBox.Show("Failed to allocate memory for DLL path in process...");
                Kernel32.CloseHandle(procHandle);
                return false;
            }

#if DEBUG
            Console.WriteLine("Allocated VMEM at: " + dllPathBaseAddrHexStr);
#endif
            UIntPtr bytesWritten = UIntPtr.Zero;
            
            if (!Kernel32.WriteProcessMemory(procHandle, dllPathBaseAddr, Encoding.ASCII.GetBytes(dllPath), dllPathLen, out bytesWritten))
            {
                MessageBox.Show("Failed to write DLL path to process memory...");
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            byte[] buffer = new byte[dllPathLen];
            UIntPtr bytesRead;

#if DEBUG
            Console.WriteLine("Successfully wrote " + bytesWritten.ToUInt32() + " to process memory");

            bool bReadMemory = Kernel32.ReadProcessMemory(procHandle, dllPathBaseAddr, buffer, buffer.Length, out bytesRead);

            if (bReadMemory)
            {
                Console.WriteLine("Read " + bytesRead + " bytes from process memory: \n");
                Console.WriteLine(" -> " + System.Text.Encoding.Default.GetString(buffer) + "\n");
            }
#endif

            IntPtr RemoteThread = Kernel32.CreateRemoteThread(procHandle, IntPtr.Zero, 0, LoadLibraryAddr, dllPathBaseAddr, 0, IntPtr.Zero);

#if DEBUG
            String LoadLibraryAddrHexStr = "0x" + LoadLibraryAddr.ToString("x8");
            String RemoteThreadHexStr = "0x" + RemoteThread.ToString("x8");

            Console.WriteLine("LoadLibrary address: " + LoadLibraryAddrHexStr);
            Console.WriteLine("RemoteThread address: " + RemoteThreadHexStr);
#endif

            if (RemoteThread == null ||
                RemoteThread == IntPtr.Zero)
            {
                MessageBox.Show("Failed to create remote thread in process");
                Kernel32.VirtualFreeEx(procHandle, dllPathBaseAddr, dllPathLen, Kernel32.MEM_RELEASE);
                Kernel32.CloseHandle(procHandle);
                return false;
            }

           ulong wfso_rc = Kernel32.WaitForSingleObject(RemoteThread, Kernel32.WFSO_INFINITE);
           UIntPtr thread_exit_code;
           Kernel32.GetExitCodeThread(RemoteThread, out thread_exit_code);

#if DEBUG
           Console.WriteLine("WaitForSingleObject returned: " + wfso_rc);
           Console.WriteLine("Thread exited with code: " + thread_exit_code);
#endif

            if (!Kernel32.VirtualFreeEx(procHandle, dllPathBaseAddr, 0, Kernel32.MEM_RELEASE))
            {
                MessageBox.Show("Failed to free memory allocated in process for DLL path...");
                Kernel32.CloseHandle(RemoteThread);
                Kernel32.CloseHandle(procHandle);
                return false;
            }

            Kernel32.CloseHandle(procHandle);

            return true;
        }
    }
}
    