using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

using YADI.Injection;
using YADI.Externals;

using System.Windows.Forms;
using System.Collections;

namespace YADI.Injection
{
    public class LoadLibrary
    {
        private int pid;
        public LoadLibrary(int pid)
        {
            this.pid = pid;
        }
        public bool Inject(String dllPath)
        {
            if (this.pid < 0)
            {
                MessageBox.Show("Invalid PID...");
                return false;
            }

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

            IntPtr Kernel32Handle = Kernel32.GetModuleHandle("kernel32.dll");
            IntPtr LoadLibraryAddr = Kernel32.GetProcAddress(Kernel32Handle, "LoadLibraryA");
            IntPtr RemoteThread = Kernel32.CreateRemoteThread(procHandle, IntPtr.Zero, 0, LoadLibraryAddr, dllPathBaseAddr, 0, IntPtr.Zero);

#if DEBUG
            String Kernel32HandleHexStr = "0x" + Kernel32Handle.ToString("x8");
            String LoadLibraryAddrHexStr = "0x" + LoadLibraryAddr.ToString("x8");
            String RemoteThreadHexStr = "0x" + RemoteThread.ToString("x8");

            Console.WriteLine("Kernel32 handle: " + Kernel32HandleHexStr);
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

           // ulong wfso_rc = Kernel32.WaitForSingleObject(RemoteThread, Kernel32.WFSO_INFINITE);
           // UIntPtr thread_exit_code;
           // Kernel32.GetExitCodeThread(RemoteThread, out thread_exit_code);

#if DEBUG
           // Console.WriteLine("WaitForSingleObject returned: " + wfso_rc);
           // Console.WriteLine("Thread exited with code: " + thread_exit_code);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
    