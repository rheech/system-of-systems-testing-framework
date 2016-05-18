using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOracleGenerator.Oracle
{
    public class MessageUnit
    {
        public string Message;
        public string From;
        public string To;

        public override string ToString()
        {
            return String.Format("{0}.{1}->{2}", From, Message, To);
        }
    }
}
