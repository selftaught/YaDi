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
            IntPtr procHandle = Kernel32.OpenProcess(Kernel32.PROCESS_INJECT, false, (uint)pid);
            IntPtr LoadLibraryAddr = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            return true;
        }
    }
}
