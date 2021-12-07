using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YADI.Structs
{
    class Module
    {
        public string Path { get; set; }
        public IntPtr BaseAddr { get; set; }
        public IntPtr EntryPoint { get; set; }
        public uint ImageSize { get; set; }

        public Module(string modPath, IntPtr baseAddr, uint imageSize, IntPtr entryPoint)
        {
            this.Path = modPath;
            this.BaseAddr = baseAddr;
            this.ImageSize = imageSize;
            this.EntryPoint = entryPoint;
        }
    }
}
