using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Helpers
{
    class Process
    {
        public static String GetFilename(System.Diagnostics.Process process)
        {
            int capacity = 256;
            StringBuilder sb = new StringBuilder(capacity);
            IntPtr hProcess = Externals.Kernel32.OpenProcess((uint)Enums.ProcessAccess.QueryLimitedInformation, false, (uint)process.Id);

            if (!Externals.Kernel32.QueryFullProcessImageName(hProcess, 0, sb, ref capacity))
            {
                return String.Empty;
            }

            return sb.ToString();
        }
    }
}
