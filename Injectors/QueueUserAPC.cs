using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using YaDi.Externals;


namespace YaDi.Injection
{
    class QueueUserAPC : Injectors.Base
    {
        private int pid;

        public QueueUserAPC(int pid)
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
