using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator.Oracle
{
    public class TestOracle : TaskNodeList
    {
        static Dictionary<TASK_OPERATOR, string> mapOperator;

        TaskNode _goalTaskNode;
        //TaskNodeList _taskNodeList;
        MessageUnitList _messages;

        public TestOracle(TaskNode goalTaskNode, MessageUnitList messages)
        {
            // define task operator only one time
            if (mapOperator == null)
            {
                MapTaskOperators();
            }

            // Must be a goal node & has child nodes
            if (goalTaskNode.Type == NODE_TYPE.GOAL &&
                    goalTaskNode.HasChildNodes)
            {
                // initialize
                _goalTaskNode = goalTaskNode;
                //_taskNodeList = goalTaskNode.ChildNodes;
                _messages = messages;

                // Add all task nodes
                this.Clear();

                foreach (TaskNode tNode in goalTaskNode.ChildNodes)
                {
                    this.Add(tNode);
                }
            }
            else
            {
                throw new ApplicationException("Test oracle must take a goal type task node which contains child nodes.");
            }
        }

        public string GoalName
        {
            get
            {
                return _goalTaskNode.Name;
            }
        }

        public MessageUnitList MessageOracle
        {
            get
            {
                return _messages;
            }
        }

        private static void MapTaskOperators()
        {
            mapOperator = new Dictionary<TASK_OPERATOR, string>();

            mapOperator.Add(TASK_OPERATOR.INTERLEAVING, "|||");
            mapOperator.Add(TASK_OPERATOR.ORDER_INDEPENDENT, "|=|");
            mapOperator.Add(TASK_OPERATOR.CHOICE, "[]");
            mapOperator.Add(TASK_OPERATOR.SEQUENTIAL, ">>");
            mapOperator.Add(TASK_OPERATOR.SEQUENTIAL_INFO, "[]>>");
            mapOperator.Add(TASK_OPERATOR.NONE, "");
        }

        public override string ToString()
        {
            StringBuilder sb;
            string sTemp;

            sb = new StringBuilder();

            sb.AppendFormat("Goal: {0}\r\n\r\n", _goalTaskNode.Name);
            sb.AppendFormat("Message Sequence:\r\n", _goalTaskNode.Name);

            for (int i = 0; i < this.Count; i++)
            {
                sTemp = String.Format("{0} {1}", _messages[i].ToString(), mapOperator[this[i].Operator]);
                sb.Append(sTemp.Trim());

                if (i < (this.Count - 1))
                {
                    sb.Append("\r\n");
                }
            }

            return sb.ToString();
        }
    }
}
