using System;

using YaDi.Externals;

namespace YaDi.Injection
{
    class SetWindowsHookEx : Injectors.Base
    {
        private int pid;

        public SetWindowsHookEx(int pid)
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
