using System;
using System.Runtime.InteropServices;
using System.Text;

namespace YADI.Externals
{
    public class Kernel32
    {
        public const uint WFSO_INFINITE = 0xFFFFFFFF;
        public const int MEM_COMMIT = 0x00001000;
        public const int MEM_RESERVE = 0x00002000;
        public const int MEM_RELEASE = 0x00008000;
        public const int PAGE_READWRITE = 0x04;
        public const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        public const int PROCESS_VM_OPERATION = 0x0008;
        public const int PROCESS_VM_READ = 0x0010;
        public const int PROCESS_VM_WRITE = 0x0020;
        public const int PROCESS_CREATE_THREAD = 0x0002;
        public const int PROCESS_QUERY_INFORMATION = 0x0400;
        public const int PROCESS_INJECT = 
            PROCESS_VM_OPERATION |
            PROCESS_CREATE_THREAD |
            PROCESS_QUERY_INFORMATION |
            PROCESS_VM_READ |
            PROCESS_VM_WRITE;

        /**
         * ThreadAccess flags
         * https://syprog.blogspot.com/2012/05/createremotethread-bypass-windows.html
         */
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200),
            THREAD_HIJACK = SUSPEND_RESUME | GET_CONTEXT | SET_CONTEXT,
        }

        /**
         * OpenProcess
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess
         */
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        /**
         * VirtualAllocEx
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualallocex
         */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        /**
         * VirtualFreeEx
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualfreeex
         */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        /**
        * VirtualQueryEx
        * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualfreeex
        */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out Structs.MemBasicInfo lpBuffer, uint dwLength);

        /**
         * VirtualProtectEx
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualprotectex
         */
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddr, uint dwSize, uint flNewProtect, out UIntPtr lpflOldProtect);

        /**
         * GetThreadContext 
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getthreadcontext
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetThreadContext(IntPtr hProcess, ref Structs.Context64 lpContext);

        /**
         * SetThreadContext
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-setthreadcontext
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetThreadContext(IntPtr hThread, ref Structs.Context64 lpContext);

        /**
         * OpenThread
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openthread
         */
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        /**
         * SuspendThread
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-suspendthread
         */
        [DllImport("kernel32.dll")]
        public static extern int SuspendThread(IntPtr hThread);

        /**
         * ResumeThread
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-resumethread
         */
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        /**
         * GetModuleHandle
         * https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea
         */
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /**
         * CloseHandle
         * https://docs.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle
         */
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

        /**
         * GetProcAddress
         * https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress
         */
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule,string procName);

        /**
         * WriteProcessMemory
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        /**
         * ReadProcessMemory
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory
         */
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out UIntPtr lpNumberOfBytesRead);

        /**
        * CreateRemoteThread
        * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createremotethread
        */
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        /**
         * WaitForSingleObject
         * https://docs.microsoft.com/en-us/windows/win32/api/synchapi/nf-synchapi-waitforsingleobject
         */
        [DllImport("kernel32.dll")]
        public static extern ulong WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        /**
         * GetExitCodeThread
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getexitcodethread
         */
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeThread(IntPtr hThread, out UIntPtr lpExitCode);

        /**
         * QueueUserAPC
         * https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-queueuserapc
         */
        [DllImport("kernel32.dll")]
        public static extern UInt32 QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

        /**
         * QueryFullProcessImageName
         * https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-queryfullprocessimagenamea
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);

        /**
         * IsWow64Process
         * https://docs.microsoft.com/en-us/windows/win32/api/wow64apiset/nf-wow64apiset-iswow64process
         */
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        /**
         * MapViewOfFile
         * https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-mapviewoffile
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, Enums.FileMapAccess dwDesiredAccess, UInt32 dwFileOffsetHigh, UInt32 dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

        /**
         * CreateFileMapping
         * https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-createfilemappinga
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, Enums.AllocationProtect flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        /**
         * GetBinaryType
         * https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getbinarytypea
         */
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetBinaryType(string lpApplicationName, out Enums.BinaryType lpBinaryType);
    }
}
