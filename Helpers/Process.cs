using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static YADI.Externals.PSAPI;

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

        public static bool IsWow64Process(System.Diagnostics.Process process)
        {
            bool bIsWow64Process = true;

            if (!Environment.Is64BitOperatingSystem)
            {
                return false;
            }


            bool bIsWow64ProcessRet = Externals.Kernel32.IsWow64Process(process.SafeHandle.DangerousGetHandle(), out bIsWow64Process);

            Console.WriteLine(bIsWow64ProcessRet);

            return bIsWow64Process;
        }

        public static List<Structs.Module> GetProcessModules(int PID)
        {
            return GetProcessModules(System.Diagnostics.Process.GetProcessById(PID));
        }

        public static List<Structs.Module> GetProcessModules(System.Diagnostics.Process proc)
        {
            List<Structs.Module> modules = new List<Structs.Module>();
            IntPtr[] modPtrs = new IntPtr[0];
            int modCount;
            int bytesNeeded;

            if (!Externals.PSAPI.EnumProcessModulesEx(proc.Handle, modPtrs, 0, out bytesNeeded, (uint)MODULE_FILTER_FLAGS.LIST_MODULES_ALL))
            {
                throw new Exception("EnumProcessModulesEx call failed!");
            }

            if (bytesNeeded == 0)
            {
                throw new Exception("Bytes needed for module list == 0!");
            }

            modCount = bytesNeeded / IntPtr.Size;
            modPtrs = new IntPtr[modCount];

            if (Externals.PSAPI.EnumProcessModulesEx(proc.Handle, modPtrs, bytesNeeded, out bytesNeeded, (uint)MODULE_FILTER_FLAGS.LIST_MODULES_ALL))
            {
                for (int i = 0; i < modCount; i++)
                {
                    StringBuilder modPathStringBuilder = new StringBuilder(50000);
                    Structs.ModuleInformation modInfo = new Structs.ModuleInformation();
                    int modInfoStructSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Structs.ModuleInformation));

                    Externals.PSAPI.GetModuleFileNameEx(proc.Handle, modPtrs[i], modPathStringBuilder, (uint)(modPathStringBuilder.Capacity));
                    Externals.PSAPI.GetModuleInformation(proc.Handle, modPtrs[i], out modInfo, (uint)modInfoStructSize);

                    String modPath = modPathStringBuilder.ToString();
                    Structs.Module mod = new Structs.Module(modPath, modInfo.BaseAddr, modInfo.ImageSize, modInfo.EntryPoint);

                    modules.Add(mod);
                }
            }

            return modules;
        }
    }
}
