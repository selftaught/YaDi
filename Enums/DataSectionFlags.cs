﻿using System;

namespace YaDi.Enums
{
    [Flags]
    public enum DataSectionFlags : uint
    {
        TypeReg = 0x00000000,
        TypeDsect = 0x00000001,
        TypeNoLoad = 0x00000002,
        TypeGroup = 0x00000004,
        TypeNoPadded = 0x00000008,
        TypeCopy = 0x00000010,
        ContentCode = 0x00000020,
        ContentInitializedData = 0x00000040,
        ContentUninitializedData = 0x00000080,
        LinkOther = 0x00000100,
        LinkInfo = 0x00000200,
        TypeOver = 0x00000400,
        LinkRemove = 0x00000800,
        LinkComDat = 0x00001000,
        NoDeferSpecExceptions = 0x00004000,
        RelativeGP = 0x00008000,
        MemPurgeable = 0x00020000,
        Memory16Bit = 0x00020000,
        MemoryLocked = 0x00040000,
        MemoryPreload = 0x00080000,
        Align1Bytes = 0x00100000,
        Align2Bytes = 0x00200000,
        Align4Bytes = 0x00300000,
        Align8Bytes = 0x00400000,
        Align16Bytes = 0x00500000,
        Align32Bytes = 0x00600000,
        Align64Bytes = 0x00700000,
        Align128Bytes = 0x00800000,
        Align256Bytes = 0x00900000,
        Align512Bytes = 0x00A00000,
        Align1024Bytes = 0x00B00000,
        Align2048Bytes = 0x00C00000,
        Align4096Bytes = 0x00D00000,
        Align8192Bytes = 0x00E00000,
        LinkExtendedRelocationOverflow = 0x01000000,
        MemoryDiscardable = 0x02000000,
        MemoryNotCached = 0x04000000,
        MemoryNotPaged = 0x08000000,
        MemoryShared = 0x10000000,
        MemoryExecute = 0x20000000,
        MemoryRead = 0x40000000,
        MemoryWrite = 0x80000000
    }
}
