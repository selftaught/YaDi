using System.Runtime.InteropServices;

namespace YADI.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader32
    {
        [FieldOffset(0)]
        public Enums.MagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public uint SizeOfCode;

        [FieldOffset(8)]
        public uint SizeOfInitializedData;

        [FieldOffset(12)]
        public uint SizeOfUninitializedData;

        [FieldOffset(16)]
        public uint AddressOfEntryPoint;

        [FieldOffset(20)]
        public uint BaseOfCode;

        // PE32 contains this additional field
        [FieldOffset(24)]
        public uint BaseOfData;

        [FieldOffset(28)]
        public uint ImageBase;

        [FieldOffset(32)]
        public uint SectionAlignment;

        [FieldOffset(36)]
        public uint FileAlignment;

        [FieldOffset(40)]
        public ushort MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public ushort MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public ushort MajorImageVersion;

        [FieldOffset(46)]
        public ushort MinorImageVersion;

        [FieldOffset(48)]
        public ushort MajorSubsystemVersion;

        [FieldOffset(50)]
        public ushort MinorSubsystemVersion;

        [FieldOffset(52)]
        public uint Win32VersionValue;

        [FieldOffset(56)]
        public uint SizeOfImage;

        [FieldOffset(60)]
        public uint SizeOfHeaders;

        [FieldOffset(64)]
        public uint CheckSum;

        [FieldOffset(68)]
        public Enums.SubSystemType Subsystem;

        [FieldOffset(70)]
        public Enums.DllCharacteristicsType DllCharacteristics;

        [FieldOffset(72)]
        public uint SizeOfStackReserve;

        [FieldOffset(76)]
        public uint SizeOfStackCommit;

        [FieldOffset(80)]
        public uint SizeOfHeapReserve;

        [FieldOffset(84)]
        public uint SizeOfHeapCommit;

        [FieldOffset(88)]
        public uint LoaderFlags;

        [FieldOffset(92)]
        public uint NumberOfRvaAndSizes;

        [FieldOffset(96)]
        public Structs.ImageDataDirectory ExportTable;

        [FieldOffset(104)]
        public Structs.ImageDataDirectory ImportTable;

        [FieldOffset(112)]
        public Structs.ImageDataDirectory ResourceTable;

        [FieldOffset(120)]
        public Structs.ImageDataDirectory ExceptionTable;

        [FieldOffset(128)]
        public Structs.ImageDataDirectory CertificateTable;

        [FieldOffset(136)]
        public Structs.ImageDataDirectory BaseRelocationTable;

        [FieldOffset(144)]
        public Structs.ImageDataDirectory Debug;

        [FieldOffset(152)]
        public Structs.ImageDataDirectory Architecture;

        [FieldOffset(160)]
        public Structs.ImageDataDirectory GlobalPtr;

        [FieldOffset(168)]
        public Structs.ImageDataDirectory TLSTable;

        [FieldOffset(176)]
        public Structs.ImageDataDirectory LoadConfigTable;

        [FieldOffset(184)]
        public Structs.ImageDataDirectory BoundImport;

        [FieldOffset(192)]
        public Structs.ImageDataDirectory IAT;

        [FieldOffset(200)]
        public Structs.ImageDataDirectory DelayImportDescriptor;

        [FieldOffset(208)]
        public Structs.ImageDataDirectory CLRRuntimeHeader;

        [FieldOffset(216)]
        public Structs.ImageDataDirectory Reserved;
    }
}
