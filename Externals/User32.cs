using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Externals
{
    class User32
    {
        /**
         * SetWindowsHookExA
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
         */
        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookExA(
            int idHook,
            IntPtr lpfn,
            IntPtr hMod,
            UInt32 dwThreadId
        );
    }
}
