using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debugger
{
    public enum LOG_LEVEL
    {
        NONE,
        ALL
    }

    public class Debug
    {
        public static LOG_LEVEL LogLevel;

        public static void Print(string format, params object[] args)
        {
            Print(LOG_LEVEL.ALL, format, args);
        }

        public static void Print(LOG_LEVEL logLv, string format, params object[] args)
        {
            if (LogLevel >= logLv)
            {
                Console.WriteLine(format, args);
            }
        }
    }
}
