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

    public class TestOracle
    {
        List<MessageUnit> _messageList;
        ORACLE_LEVEL _oracleLevel;

        public TestOracle()
        {
        }

        public TestOracle(MessageUnit[] messages)
        {
            _messageList = new List<MessageUnit>(messages);
        }

        public static TestOracle[] FromTaskSequenceSet(TaskSequenceSet tSet)
        {
            List<TestOracle> tOracleList;
            List<MessageUnit> msgList;
            MessageUnit msgUnit;

            tOracleList = new List<TestOracle>();

            for (int i = 0; i < tSet.Length; i++)
            {
                msgList = new List<MessageUnit>();

                for (int j = 0; j < tSet[i].Length; j++)
                {
                    msgUnit = new MessageUnit();
                    msgUnit.Message = tSet[i][j].ToString();

                    msgList.Add(msgUnit);
                }

                tOracleList.Add(new TestOracle(msgList.ToArray()));
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

                throw new Exception("Invalid index");
            }
            set
            {
                if (_messageList != null)
                {
                    _messageList[index] = value;
                }

                throw new Exception("Invalid index");
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
