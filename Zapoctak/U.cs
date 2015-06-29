using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak
{
    static class U //utility class
    {
        public static float SQRT2DIV2 = (float)(Math.Sqrt(2) / 2);

        public static Random ran = new Random();

        public static String ToString(object obj)
        {
            return (obj ?? "null").ToString();
        }

        public static double Clamp(double val, double min, double max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static int Clamp(int val, int min, int max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static int Mod(int n, int m)
        {
            int ret = n % m;
            return ret >= 0 ? ret : m + ret;
        }
    }
}
