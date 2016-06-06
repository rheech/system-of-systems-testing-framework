using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOracleGenerator.Oracle
{
    public class MessageUnitList : List<MessageUnit>
    {
        private static Predicate<MessageUnit> ByMessage(MessageUnit message)
        {
            return delegate(MessageUnit msgUnit)
            {
                return msgUnit == message;
            };
        }


        public int OccurrenceOf(MessageUnit message)
        {
            return this.FindAll(ByMessage(message)).Count;
        }
    }
}
