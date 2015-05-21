using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak
{
    static class Log
    {
        public const int LV_NONE = -1, LV_ERROR = 0, LV_WARNING = 1,
                         LV_INFO = 2, LV_BATTLE = 3, LV_DEBUG =5, LV_ALL = 5;
        public static int lv = LV_ALL;

        public static void e(String msg)
        {
            if(lv >= LV_ERROR) Console.WriteLine("Error: " + msg);
        }

        public static void e(String msg, Exception ex)
        {
            if (lv >= LV_ERROR)
            {
                Console.WriteLine("Error with exeption: " + msg);
                Console.WriteLine("Exception: " + ex);
            }
        }

        public static void w(String msg)
        {
            if (lv >= LV_WARNING) Console.WriteLine("Warning: " + msg);
        }

        public static void i(String msg)
        {
            if (lv >= LV_INFO) Console.WriteLine("Info: " + msg);
        }

        public static void b(String msg)
        {
            if (lv >= LV_BATTLE) Console.WriteLine("Battle log: " + msg);
        }

        public static void d(String msg)
        {
            if (lv >= LV_DEBUG) Console.WriteLine("Debug: " + msg);
        }
    }
}
