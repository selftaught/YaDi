using System;

namespace YADI.Injectors
{
    public class Base
    {
        public virtual bool Inject(String dllPath)
        {
            throw new Exception("Inject method override undefined");
        }
    }
}
