using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

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

        public static Enums.BinaryType GetBinaryType(int pid)
        {
            String path = GetFilename(System.Diagnostics.Process.GetProcessById(pid));
            Enums.BinaryType binaryType;
            Externals.Kernel32.GetBinaryType(path, out binaryType);
            return binaryType;
        }

        public static Enums.BinaryType GetBinaryType(String path)
        {
            Enums.BinaryType binaryType;
            Externals.Kernel32.GetBinaryType(path, out binaryType);
            return binaryType;
        }

        public static bool Is64Bit(String path)
        {
            bool bIs64Bit = false;

            if (Environment.Is64BitOperatingSystem)
            {
                if (GetBinaryType(path) == Enums.BinaryType.SCS_64BIT_BINARY)
                {
                    bIs64Bit = true;
                }
            }

            return bIs64Bit;
        }

        public static bool Is64Bit(int pid)
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(pid);
            return Is64Bit(GetFilename(proc));
        }

        public static bool Is32Bit(String path)
        {
            bool bIs32Bit = false;

            if (Environment.Is64BitOperatingSystem)
            {
                if (GetBinaryType(path) == Enums.BinaryType.SCS_32BIT_BINARY)
                {
                    bIs32Bit = true;
                }
            }

            return bIs32Bit;
        }

        public static bool Is32Bit(int pid)
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcessById(pid);
            return Is32Bit(GetFilename(proc));
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

            return modules;
        }
    }
}
