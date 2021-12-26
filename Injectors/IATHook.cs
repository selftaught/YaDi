using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Injection
{
    class IATHook
    {
        uint pid;

        public IATHook(int pid)
        {
            this.pid = (uint)pid;
        }

        public bool Inject(String dllPath)
        {

            return true;
        }
    }
}
