using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public class TaskNode : TaskInterface
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

        public void TraverseChildNodes(ref TaskNodeTraversalCallback nodeAction)
        {
            XmlNodeList nodes;
            //TRAVERSE_OPTION traverseOption;
            TaskNode taskNode;

            nodes = _xmlNode.ChildNodes;
            //traverseOption = TRAVERSE_OPTION.ALL;

            foreach (XmlNode node in nodes)
            {
                // Convert to TaskNode
                taskNode = new TaskNode(node);

                TraverseChildNodesInternal(taskNode, ref nodeAction);

                /*if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                {
                    break;
                }*/
            }
        }

        private void TraverseChildNodesInternal(TaskNode taskNode, ref TaskNodeTraversalCallback nodeAction)
        {
            // If goal model, callback
            if (taskNode.Type == NODE_TYPE.GOAL)
            {
                nodeAction(taskNode);

                // traverse children
                if (taskNode.HasChildNodes)
                {
                    foreach (TaskNode node in taskNode.ChildNodes)
                    {
                        TraverseChildNodesInternal(node, ref nodeAction);
                    }
                }
                else
                {
                    throw new Exception("Task model definition error. Goal node must have children.");
                }
            }


            /*
            if (taskNode.HasChildNodes && (traverseOption == TRAVERSE_OPTION.ALL))
            {
                foreach (XmlNode x in taskNode.ChildNodes)
                {
                    traverseOption = recTraverseXmlNode(x, ref nodeAction);

                    if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                    {
                        break;
                    }
                }
            }*/
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

        public TaskNodeList ChildNodes
        {
            get
            {
                List<TaskNode> taskNodeList;

                taskNodeList = new List<TaskNode>();

                if (_xmlNode.HasChildNodes)
                {
                    foreach (XmlNode n in _xmlNode.ChildNodes)
                    {
                        taskNodeList.Add(new TaskNode(n));
                    }

                    return new TaskNodeList(taskNodeList);
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
    }
}
