using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator.Oracle
{
    public enum ORACLE_LEVEL
    {
        TASK,
        ROLE,
        AGENT
    }

    public class TestOracle2
    {
        List<MessageUnit> _messageList;
        ORACLE_LEVEL _oracleLevel;

        public TestOracle2()
        {
        }

        public TestOracle2(MessageUnit[] messages)
        {
            _messageList = new List<MessageUnit>(messages);
        }

        public static TestOracle2[] FromTaskSequenceSet(TaskSequenceSet tSet)
        {
            List<TestOracle2> tOracleList;
            List<MessageUnit> msgList;
            MessageUnit msgUnit;

            tOracleList = new List<TestOracle2>();

            for (int i = 0; i < tSet.Length; i++)
            {
                msgList = new List<MessageUnit>();

                for (int j = 0; j < tSet[i].Length; j++)
                {
                    msgUnit = new MessageUnit();
                    msgUnit.Message = tSet[i][j].ToString();

                    msgList.Add(msgUnit);
                }

                tOracleList.Add(new TestOracle2(msgList.ToArray()));
            }

            return tOracleList.ToArray();
        }

        public int Length
        {
            get
            {
                if (_messageList != null)
                {
                    return _messageList.Count;
                }

                return 0;
            }
        }

        public MessageUnit[] ToMessageList()
        {
            return _messageList.ToArray();
        }

        public MessageUnit this[int index]
        {
            get
            {
                if (_messageList != null)
                {
                    return _messageList[index];
                }

                throw new ApplicationException("Invalid index");
            }
            set
            {
                if (_messageList != null)
                {
                    _messageList[index] = value;
                }

                throw new ApplicationException("Invalid index");
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (MessageUnit m in _messageList)
            {
                sb.AppendFormat("{0}\r\n", m);
            }

            return sb.ToString();
        }
    }
}
