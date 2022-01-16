using System;
using System.Runtime.InteropServices;

namespace YaDi.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_NT_HEADERS64
    {
        [FieldOffset(0)]
        public UInt32 Signature;

        [FieldOffset(4)]
        public IMAGE_FILE_HEADER FileHeader;

        [FieldOffset(24)]
        public Structs.ImageOptionalHeader64 OptionalHeader;

        private string _Signature
        {
            get { return Signature.ToString(); }
        }

        public bool isValid
        {
            get { return _Signature == "PE\0\0" && OptionalHeader.Magic == Enums.MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC; }
        }
    }
}
