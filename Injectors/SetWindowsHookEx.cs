using System;


namespace YADI.Injection
{
    class SetWindowsHookEx : Injectors.Base
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
