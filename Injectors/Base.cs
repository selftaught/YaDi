using System;

namespace YaDi.Injectors
{
    public class Base
    {
        public virtual bool Inject(String szDllPath)
        {
            throw new Exception("Inject Me`thod Unimplemented");
        }
    }
}
