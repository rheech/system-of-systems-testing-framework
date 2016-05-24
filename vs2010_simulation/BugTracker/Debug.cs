using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker
{
    public class Debug
    {
        private enum LOG_LEVEL
        {
            ENABLE,
            DISABLE
        }

        private static LOG_LEVEL _logLv = LOG_LEVEL.ENABLE;

        public static void WriteLine(string format, params object[] arg)
        {
            if (_logLv == LOG_LEVEL.ENABLE)
            {
                Console.WriteLine(format, arg);
            }
        }
    }
}
