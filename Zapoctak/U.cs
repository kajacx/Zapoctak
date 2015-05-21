using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak
{
    class U
    {
        public static Random ran = new Random();

        public static String ToString(object obj)
        {
            return (obj ?? "null").ToString();
        }
    }
}
