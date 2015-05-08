using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak
{
    static class Log
    {
        public static void e(String msg)
        {
            Console.WriteLine("Error: " + msg);
        }

        public static void e(String msg, Exception ex)
        {
            Console.WriteLine("Error with exeption: " + msg);
            Console.WriteLine("Exception: " + ex);
        }

        public static void w(String msg)
        {
            Console.WriteLine("Warning: "+msg);
        }

        public static void d(String msg)
        {
            Console.WriteLine("Debug: " + msg);
        }
    }
}
