using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public enum TASK_OPERATOR
    {
        NONE,
        ENABLE,
        CHOICE,
        PARALLEL
    }

    public enum NODE_TYPE
    {
        NONE,
        GOAL,
        TASK
    }

    public class TaskNode2 : TaskInterface
    {
        public string Name;
        public NODE_TYPE Type;
        private TASK_OPERATOR _operator;
        public bool isLeaf;
        public XmlNode xmlNode;

        public static TASK_OPERATOR ParseOperator(string operatorName)
        {
            TASK_OPERATOR tOperator;

            switch (operatorName.ToLower())
            {
                case "enable":
                    tOperator = TASK_OPERATOR.ENABLE;
                    break;
                case "choice":
                    tOperator = TASK_OPERATOR.CHOICE;
                    break;
                case "parallel":
                    tOperator = TASK_OPERATOR.PARALLEL;
                    break;
                default:
                    tOperator = TASK_OPERATOR.NONE;
                    break;
            }

            return tOperator;
        }

        public static NODE_TYPE ParseNodeType(string typeName)
        {
            NODE_TYPE nType;

            switch (typeName.ToLower())
            {
                case "goal":
                    nType = NODE_TYPE.GOAL;
                    break;
                case "task":
                    nType = NODE_TYPE.TASK;
                    break;
                default:
                    nType = NODE_TYPE.NONE;
                    break;
            }

            return nType;
        }

        public bool hasChildNode
        {
            get
            {
                return !isLeaf;
            }
        }

        public bool hasNextNode
        {
            get
            {
                return Operator != TASK_OPERATOR.NONE;
            }
        }

        public bool hasNextChildNode
        {
            get
            {
                return !(isLeaf && (Operator == TASK_OPERATOR.NONE));
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public TASK_OPERATOR Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }
    }
}
