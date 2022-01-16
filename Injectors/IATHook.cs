using System;

// reference: https://www.ired.team/offensive-security/code-injection-process-injection/import-adress-table-iat-hooking

namespace YaDi.Injection
{
    class IATHook : Injectors.Base
    {
        uint pid;

        public IATHook(int nPid)
        {
            this.pid = (uint)nPid;
        }

        public override bool Inject(String sDllPath)
        {
            /**
             * @TODO
             */
            return true;
        }
    }
}
