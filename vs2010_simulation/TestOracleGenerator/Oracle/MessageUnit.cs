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

        public static bool operator ==(MessageUnit a, MessageUnit b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MessageUnit a, MessageUnit b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            bool bEquals;
            MessageUnit msg;

            if (obj is MessageUnit)
            {
                msg = (MessageUnit)obj;
                bEquals = (Message == msg.Message && From == msg.From && To == msg.To);

                return bEquals;
            }

            return base.Equals(obj);
        }
    }
}
