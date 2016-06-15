using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TestOracleGenerator.Oracle;

namespace TestOracleGenerator.Xml
{
    public enum TASK_OPERATOR
    {
        NONE,
        INTERLEAVING, // Independent Concurrency, |||
        CHOICE, // Choice, []
        SYNCHRONIZATION, // Synchronization, |[]|
        CONCURRENCY, // Concurrency ||
        SEQUENTIAL, // Enabling, >>
        SEQUENTIAL_INFO, // Enabling Info, []>>
        ORDER_INDEPENDENT // Order Independent |=|
    }

    public enum NODE_TYPE
    {
        NONE,
        GOAL,
        TASK
    }

    public class TaskNode
    {
        private XmlNode _xmlNode;

        public TaskNode(XmlNode xmlNode)
        {
            _xmlNode = xmlNode;
        }

        public static TASK_OPERATOR ParseOperator(string operatorName)
        {
            TASK_OPERATOR tOperator;

            switch (operatorName.ToLower())
            {
                case "interleaving":
                    tOperator = TASK_OPERATOR.INTERLEAVING;
                    break;
                case "choice":
                    tOperator = TASK_OPERATOR.CHOICE;
                    break;
                case "synchronization":
                    tOperator = TASK_OPERATOR.SYNCHRONIZATION;
                    break;
                case "concurrency":
                    tOperator = TASK_OPERATOR.CONCURRENCY;
                    break;
                case "sequential":
                    tOperator = TASK_OPERATOR.SEQUENTIAL;
                    break;
                case "sequentialinfo":
                    tOperator = TASK_OPERATOR.SEQUENTIAL_INFO;
                    break;
                case "orderindependent":
                    tOperator = TASK_OPERATOR.ORDER_INDEPENDENT;
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
                case "interaction":
                    nType = NODE_TYPE.TASK;
                    break;
                default:
                    nType = NODE_TYPE.NONE;
                    break;
            }

            return nType;
        }

        public void TraverseChildNodes(ref TaskNodeTraversalCallback nodeAction)
        {
            XmlNodeList nodes;
            TaskNode taskNode;

            nodes = _xmlNode.ChildNodes;

            foreach (XmlNode node in nodes)
            {
                // Convert to TaskNode
                taskNode = new TaskNode(node);

                if (!TraverseChildNodesInternal(taskNode, ref nodeAction))
                {
                    break;
                }
            }
        }

        private bool TraverseChildNodesInternal(TaskNode taskNode, ref TaskNodeTraversalCallback nodeAction)
        {
            bool bContinueTraversing;

            bContinueTraversing = true;

            // If goal model, callback
            if (taskNode.Type == NODE_TYPE.GOAL)
            {
                bContinueTraversing = nodeAction(taskNode);

                if (bContinueTraversing)
                {
                    // traverse children
                    if (taskNode.HasChildNodes)
                    {
                        foreach (TaskNode node in taskNode.ChildNodes)
                        {
                            bContinueTraversing = TraverseChildNodesInternal(node, ref nodeAction);

                            if (!bContinueTraversing)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Task model definition error. Goal node must have children.");
                    }
                }
            }

            return bContinueTraversing;
        }

        public string Name
        {
            get
            {
                string name;

                name = "";

                if (_xmlNode.Attributes["name"] != null)
                {
                    name = _xmlNode.Attributes["name"].InnerText;
                }

                return name;
            }
        }

        public int RecursionCount
        {
            get
            {
                string sCount;
                int numCount;

                sCount = "";

                // check if count attribute exists
                if (_xmlNode.Attributes["count"] != null)
                {
                    sCount = _xmlNode.Attributes["count"].InnerText;

                    // check if input is valid
                    if (Int32.TryParse(sCount, out numCount))
                    {
                        return numCount;
                    }
                    else if (sCount == "*")
                    {
                        return -1;
                    }
                    else
                    {
                        throw new ApplicationException("Invlid count detected in task node.");
                    }
                }

                return -1; // Default is the infinite occurrence
            }
        }

        public TaskNodeList ChildNodes
        {
            get
            {
                TaskNodeList taskNodeList;

                taskNodeList = new TaskNodeList();

                if (_xmlNode.HasChildNodes)
                {
                    foreach (XmlNode n in _xmlNode.ChildNodes)
                    {
                        taskNodeList.Add(new TaskNode(n));
                    }

                    return taskNodeList;
                }

                return null;
            }
        }

        public NODE_TYPE Type
        {
            get
            {
                NODE_TYPE nodeType;

                nodeType = NODE_TYPE.NONE;

                if (_xmlNode.Attributes["type"] != null)
                {
                    nodeType = TaskNode.ParseNodeType(_xmlNode.Attributes["type"].InnerText);
                }

                return nodeType;
            }
        }

        public bool HasChildNodes
        {
            get
            {
                return _xmlNode.HasChildNodes;
            }
        }

        public XmlNode Markup
        {
            get
            {
                return _xmlNode;
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
                TASK_OPERATOR taskOperator;

                taskOperator = TASK_OPERATOR.NONE;

                if (_xmlNode.Attributes["operator"] != null)
                {
                    taskOperator = TaskNode.ParseOperator(_xmlNode.Attributes["operator"].InnerText);
                }

                return taskOperator;
            }
        }

        public bool IsSameContent(object obj)
        {
            bool bEquals;
            TaskNode tNode;

            if (obj is TaskNode)
            {
                tNode = (TaskNode)obj;
                bEquals = (Name == tNode.Name && Operator == tNode.Operator && RecursionCount == tNode.RecursionCount);

                return bEquals;
            }

            return base.Equals(obj);
        }
    }
}
