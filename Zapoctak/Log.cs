using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak
{
    static class Log
    {
        public const int LV_NONE = 0, LV_ERROR = 1, LV_WARNING = 2, LV_INFO = 4,
                         LV_BATTLE = 8, LV_DEBUG = 16, LV_ALL = 31;
        public static int lv = LV_ALL;

        public static void E(String msg)
        {
            if ((lv & LV_ERROR) > 0) Console.WriteLine("Error: " + msg);
        }

        public static void E(String msg, Exception ex)
        {
            if ((lv & LV_ERROR) > 0)
            {
                Console.WriteLine("Error with exeption: " + msg);
                Console.WriteLine("Exception: " + ex);
            }
        }

        public static void W(String msg)
        {
            if ((lv & LV_WARNING) > 0) Console.WriteLine("Warning: " + msg);
        }

        public static void I(String msg)
        {
            if ((lv & LV_INFO) > 0) Console.WriteLine("Info: " + msg);
        }

        public static void B(String msg)
        {
            if ((lv & LV_BATTLE) > 0) Console.WriteLine("Battle: " + msg);
        }

        public static void D(String msg)
        {
            if ((lv & LV_DEBUG) > 0) Console.WriteLine("Debug: " + msg);
        }

        public static void D(String format, params object[] args)
        {
            if ((lv & LV_DEBUG) > 0) Console.WriteLine("Debug: " + format, args);
        }
    }
}
