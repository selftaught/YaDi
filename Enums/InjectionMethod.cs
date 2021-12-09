using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Enums
{
    public enum InjectionMethod : ushort
    {
        LoadLibrary = 0,
        SetWindowsHook = 1,
        ThreadHijack = 2,
        QueueUserAPC = 3,
        IATHook = 4,
    }
}
