using System;


namespace YADI.Injection
{
    class IATHook : Injectors.Base
    {
        uint pid;

        public IATHook(int pid)
        {
            this.pid = (uint)pid;
        }

        public override bool Inject(String dllPath)
        {

            return true;
        }
    }
}
