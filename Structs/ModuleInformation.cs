using System;
using System.Runtime.InteropServices;

namespace YaDi.Structs
{
    /**
     * ModuleInformation
     */
    [StructLayout(LayoutKind.Sequential)]
    public struct ModuleInformation
    {
        public IntPtr BaseAddr;
        public uint ImageSize;
        public IntPtr EntryPoint;
    }
}
