using System;

namespace YADI.Helpers
{
    class Misc
    {
        public static double Epoch()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return t.TotalSeconds;
        }
    }
}
