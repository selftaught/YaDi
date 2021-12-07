using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Structs
{
    public struct MemBasicInfo
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public Enums.AllocationProtect AllocationProtect;
        public IntPtr RegionSize;
        public Enums.State State;
        public Enums.AllocationProtect Protect;
        public Enums.Type Type;
    }
}
