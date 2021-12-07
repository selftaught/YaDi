﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YADI.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 16)]
    public struct XSaveFormat64
    {
        public ushort ControlWord;
        public ushort StatusWord;
        public byte TagWord;
        public byte Reserved1;
        public ushort ErrorOpcode;
        public uint ErrorOffset;
        public ushort ErrorSelector;
        public ushort Reserved2;
        public uint DataOffset;
        public ushort DataSelector;
        public ushort Reserved3;
        public uint MxCsr;
        public uint MxCsr_Mask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Structs.M128A[] FloatRegisters;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Structs.M128A[] XmmRegisters;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        public byte[] Reserved4;
    }
}
