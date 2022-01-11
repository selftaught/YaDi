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

            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById((int)pid);

            IntPtr pBaseAddr = Helpers.Process.GetBaseAddr(proc);
            String sProcFile = Helpers.Process.GetFilename((int)pid);

            PeNet.PeFile pe = new PeNet.PeFile(sProcFile);

            Console.WriteLine(pBaseAddr);
            Console.WriteLine(sProcFile);

            PeNet.Header.Pe.ImportFunction[] impFuncs = pe.ImportedFunctions;

            foreach (PeNet.Header.Pe.ImportFunction impFunc in impFuncs)
            {
                Console.WriteLine(impFunc.Name + " (" + impFunc.IATOffset.ToString() + ")");
            }

            foreach (PeNet.Header.Pe.ImageImportDescriptor iid in pe.ImageImportDescriptors)
            {
                Console.WriteLine(iid.Name);
            }


            foreach (PeNet.Header.Pe.ImageSectionHeader ish in pe.ImageSectionHeaders)
            {
                Console.WriteLine(ish.Name + ", VA: " + ish.VirtualAddress.ToString() + ", VSIZE: " + ish.VirtualSize + ", RAW SIZE: " + ish.SizeOfRawData + ", IMG BASE: " + ish.ImageBaseAddress.ToString() + ", PROC BA: " + pBaseAddr.ToString());

                Console.WriteLine(ish.PointerToRawData.ToString());
                Console.WriteLine(ish.PointerToRawData);
            }

            return true;
        }
    }
}
