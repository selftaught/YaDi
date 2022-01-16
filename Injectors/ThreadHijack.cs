using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using YaDi.Externals;

namespace YaDi.Injection
{
    class ThreadHijack : Injectors.Base
    {
        private int pid;

        public ThreadHijack(int pid)
        {
            this.pid = pid;
        }

        public override bool Inject(String dllPath)
        {
            /**
             * @TODO
             */
            return true;
        }
    }
}
