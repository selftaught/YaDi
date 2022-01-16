using System;

/**
 * YaDi.Structs.LvItem
 * http://csharphelper.com/blog/2018/03/display-icons-next-to-listview-sub-items-in-c/
 */
namespace YaDi.Structs
{
    public struct LvItem
    {
        public UInt32 uiMask;
        public Int32 iItem;
        public Int32 iSubItem;
        public UInt32 uiState;
        public UInt32 uiStateMask;
        public string pszText;
        public Int32 cchTextMax;
        public Int32 iImage;
        public IntPtr lParam;
    };
}
