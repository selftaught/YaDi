using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Injection
{
    class SetWindowsHookEx
    {
        private int pid;

        public SetWindowsHookEx(int pid)
        {
            this.pid = pid;
        }

        public bool Inject(String dllPath)
        {

            return true;
        }
    }
}
