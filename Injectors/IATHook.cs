using System;

// reference: https://www.ired.team/offensive-security/code-injection-process-injection/import-adress-table-iat-hooking

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
