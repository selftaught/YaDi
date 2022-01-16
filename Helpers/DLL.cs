using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaDi.Helpers
{
    public class DLL
    {
        public static bool Is32Bit(String dllPath)
        {
            Enums.MachineType machineType = GetDllMachineType(dllPath);

            switch (machineType)
            {
                case Enums.MachineType.IMAGE_FILE_MACHINE_I386: { return true; };
                case Enums.MachineType.IMAGE_FILE_MACHINE_M32R: { return true; };
            }

            return false;
        }

        public static bool Is64Bit(String dllPath)
        {
            Enums.MachineType machineType = GetDllMachineType(dllPath);

            switch (machineType)
            {
                case Enums.MachineType.IMAGE_FILE_MACHINE_IA64: { return true; };
                case Enums.MachineType.IMAGE_FILE_MACHINE_AMD64: { return true; };
            }

            return false;
        }

        private static Enums.MachineType GetDllMachineType(string dllPath)
        {
            FileStream fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            fs.Seek(0x3c, SeekOrigin.Begin);
            Int32 peOffset = br.ReadInt32();
            fs.Seek(peOffset, SeekOrigin.Begin);
            UInt32 peHead = br.ReadUInt32();

            if (peHead != 0x00004550) // "PE\0\0", little-endian
            {
                throw new Exception("Can't find PE header");
            }

            Enums.MachineType machineType = (Enums.MachineType)br.ReadUInt16();

            br.Close();
            fs.Close();

            return machineType;
        }
    }
}
