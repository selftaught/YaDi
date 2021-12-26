using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YADI.Externals
{
    public class PSAPI
    {
        /**
        * MODULE_FILTER_FLAGS
        */
        internal enum MODULE_FILTER_FLAGS : uint
        {
            LIST_MODULES_DEFAULT = 0x00,
            LIST_MODULES_32BIT = 0x01,
            LIST_MODULES_64BIT = 0x02,
            LIST_MODULES_ALL = 0x03
        }

        /**
        * GetModuleFileNameEx
        * https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmodulefilenameexa
        */
        [DllImport("psapi.dll")]
        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        /**
         * GetModuleInformation
         * https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmoduleinformation
         */
        [DllImport("psapi.dll")]
        private static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out Structs.ModuleInformation lpModInfo, uint cb);

        /**
         * EnumProcessModulesEx
         * https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex
         */
        [DllImport("psapi.dll")]
        public static extern bool EnumProcessModulesEx(
            IntPtr hProcess,
            IntPtr[] lphModule,
            int cb,
            out int lpcbNeeded,
            uint dwFilterFlag
        );

        static List<Structs.Module> GetProcessModules(Process proc)
        {
            List<Structs.Module> modules = new List<Structs.Module>();
            IntPtr[] modPtrs = new IntPtr[0];
            int modCount;
            int bytesNeeded;

            if (!EnumProcessModulesEx(proc.Handle, modPtrs, 0, out bytesNeeded, (uint)MODULE_FILTER_FLAGS.LIST_MODULES_ALL))
            {
                MessageBox.Show("Couldn't EnumProcessModulesEx...");
                return modules;
            }

            if (bytesNeeded == 0)
            {
                MessageBox.Show("Bytes needed for module list returned 0...");
                return modules;
            }

            modCount = bytesNeeded / IntPtr.Size;
            modPtrs = new IntPtr[modCount];

            if (EnumProcessModulesEx(proc.Handle, modPtrs, bytesNeeded, out bytesNeeded, (uint)MODULE_FILTER_FLAGS.LIST_MODULES_ALL))
            {
                for (int i = 0; i < modCount; i++)
                {
                    StringBuilder modPathStringBuilder = new StringBuilder(50000);
                    Structs.ModuleInformation modInfo = new Structs.ModuleInformation();
                    int modInfoStructSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Structs.ModuleInformation));
                    GetModuleFileNameEx(proc.Handle, modPtrs[i], modPathStringBuilder, (uint)(modPathStringBuilder.Capacity));
                    GetModuleInformation(proc.Handle, modPtrs[i], out modInfo, (uint)modInfoStructSize);

                    String modPath = modPathStringBuilder.ToString();
                    Structs.Module mod = new Structs.Module(modPath, modInfo.BaseAddr, modInfo.ImageSize, modInfo.EntryPoint);
                    modules.Add(mod);
                }
            }

            return modules;
        }
    }
}
