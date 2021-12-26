using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Externals
{
    public class User32
    {
        public const Int32 LVM_FIRST = 0x1000;
        public const Int32 LVM_SETITEM = LVM_FIRST + 6;
        public const Int32 LVIF_IMAGE = 0x2;

        public const int LVW_FIRST = 0x1000;
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVW_FIRST + 54;
        public const int LVM_GETEXTENDEDLISTVIEWSTYLE = LVW_FIRST + 55;

        public const int LVS_EX_SUBITEMIMAGES = 0x2;

        /**
         * SetWindowsHookExA
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
         */
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookExA(
            int idHook,
            IntPtr lpfn,
            IntPtr hMod,
            UInt32 dwThreadId
        );

        /**
         * SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
         */
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        /**
         * SendMessage(IntPtr hWnd, uint Msg, int wParam, ref Structs.LvItem item_info);
         * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
         */
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, ref Structs.LvItem item_info);
    }
}
