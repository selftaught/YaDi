using System;

// reference: https://www.ired.team/offensive-security/code-injection-process-injection/import-adress-table-iat-hooking

namespace YADI.Injection
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
             * 1. Get IAT
             * 2. Find a function in the IAT to hook
             * 3. Save the memory address of that function.
             * 4. Create a hook/intercept function to replace the
             *    function found in step #2.
             * 5. Replace function found in step #2
             *    with the hook/intercept function address.
             */
            return true;
        }
    }
}
