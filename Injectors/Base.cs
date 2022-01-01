using System;

namespace YADI.Injectors
{
    public class Base
    {
        public virtual bool Inject(String szDllPath)
        {
            throw new Exception("Inject Method Unimplemented");
        }
    }
}
