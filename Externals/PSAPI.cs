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
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        /**
         * GetModuleInformation
         * https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmoduleinformation
         */
        [DllImport("psapi.dll")]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out Structs.ModuleInformation lpModInfo, uint cb);

        /**
         * EnumProcessModulesEx
         * https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex
         */
        [DllImport("psapi.dll")]
        public static extern bool EnumProcessModulesEx(IntPtr hProcess, IntPtr[] lphModule, int cb, out int lpcbNeeded, uint dwFilterFlag);

    }
}
