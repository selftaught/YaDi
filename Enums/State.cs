using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Enums
{
    public enum State : uint
    {
        MEM_COMMIT  = 0x1000,
        MEM_FREE    = 0x10000,
        MEM_RESERVE = 0x2000
    }
}
