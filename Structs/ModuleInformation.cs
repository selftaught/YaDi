using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Structs
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
