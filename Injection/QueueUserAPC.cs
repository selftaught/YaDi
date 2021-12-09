using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YADI.Externals;


namespace YADI.Injection
{
    class QueueUserAPC
    {
        private int pid;

        public QueueUserAPC(int pid)
        {
            this.pid = pid;
        }

        public bool Inject(String dllPath)
        {
            IntPtr procHandle = Kernel32.OpenProcess(
                Kernel32.PROCESS_CREATE_THREAD |
                Kernel32.PROCESS_QUERY_INFORMATION |
                Kernel32.PROCESS_VM_OPERATION |
                Kernel32.PROCESS_VM_WRITE |
                Kernel32.PROCESS_VM_READ,
                false, (uint)this.pid);


            if (procHandle == null)
            {
                MessageBox.Show("Couldn't get process handle...");
                return false;
            }

            return true;
        }
    }
}
