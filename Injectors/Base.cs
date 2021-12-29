using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
