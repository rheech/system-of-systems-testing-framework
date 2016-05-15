using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TaskModelReader
{
    class TaskModel : XmlParser
    {
        private List<XmlNode> testCase;
        public delegate bool dEachNodeAction(XmlNode xmlNode, bool isLeaf);

        public TaskModel(string file)
            : base(file, "/Diagram/node")
        {
            testCase = new List<XmlNode>();
        }
        
        public string[] RetrieveGoalList()
        {
            dEachNodeAction FUNC_RETRIEVE_GOAL;
            List<string> goalList = new List<string>();

            // Define goal procedure for the node traversal
            FUNC_RETRIEVE_GOAL = new dEachNodeAction((xmlNode, isLeaf) =>
            {
                //Console.WriteLine(xmlNode.Attributes["name"].InnerText);

                if (xmlNode.Attributes["type"].InnerText == "goal")
                {
                    goalList.Add(xmlNode.Attributes["name"].InnerText);
                }

                return false;
            });

            TraverseAllNodes(ref FUNC_RETRIEVE_GOAL);

            return goalList.ToArray();
        }

        public TaskSequence RetrieveTaskSequence(string goalName)
        {
            //TreeLogic tl = new TreeLogic(RootNodes[0]);
            dEachNodeAction FUNC_RETRIEVE_TASK;

            bool bFound = false;

            TaskSequence taskSeq = new TaskSequence();
            List<string> taskList = new List<string>();
            TaskNode previousNode = null;

            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new dEachNodeAction((xmlNode, isLeaf) =>
            {
                TaskNode currentNode;
                currentNode = new TaskNode();

                currentNode.Name = xmlNode.Attributes["name"].InnerText;

                currentNode.Type = NODE_TYPE.NONE;

                if (xmlNode.Attributes["type"] != null)
                {
                    currentNode.Type = TaskNode.ParseNodeType(xmlNode.Attributes["type"].InnerText);
                }

                currentNode.Operator = TASK_OPERATOR.NONE;

                if (xmlNode.Attributes["operator"] != null)
                {
                    currentNode.Operator = TaskNode.ParseOperator(xmlNode.Attributes["operator"].InnerText);
                }

                currentNode.isLeaf = isLeaf;
                Console.WriteLine(currentNode.Name);

                // Found Goal
                if (currentNode.Name == goalName)
                {
                    bFound = true;
                    previousNode = null;
                    //tl = new TreeLogic(xmlNode);
                    //goalList.Add(xmlNode.Attributes["name"].InnerText);
                    return false;
                }
                
                // after goal has found
                if (bFound)
                {
                    // If first node
                    if (previousNode == null)
                    {
                        previousNode = currentNode;
                        taskList = new List<string>();
                        taskList.Add(currentNode.Name);
                    }
                    else
                    {
                        switch (previousNode.Operator)
                        {
                            case TASK_OPERATOR.ENABLE:
                                taskList.Add(currentNode.Name);
                                break;
                            case TASK_OPERATOR.CHOICE:
                                taskSeq.AddList(taskList);
                                taskList = new List<string>();
                                taskList.Add(currentNode.Name);
                                break;
                            case TASK_OPERATOR.PARALLEL:
                                break;
                            default:
                                taskList.Add(currentNode.Name);
                                break;
                        }

                        previousNode = currentNode;
                    }

                    // if a goal node
                    if (currentNode.Type == NODE_TYPE.GOAL)
                    {
                        TaskSequence seq;
                        seq = RetrieveTaskSequence(currentNode.Name);
                    }

                    // if last node
                    if (!currentNode.hasNextChildNode)
                    {
                        taskSeq.AddList(taskList);
                        return true;
                    }
                }

                return false;
            });

            TraverseAllNodes(ref FUNC_RETRIEVE_TASK);

            return taskSeq;
        }

        public string test()
        {
            XmlNodeList nodes = RootNodes;

            /*foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("node"))
                {
                    if (n.Attributes["type"].InnerText == "goal" && n.Attributes["name"].InnerText == "FindAvailableHospital")
                    {
                        return node.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return "[Find]";*/

            //return recFindNode(nodes[0]).Attributes["type"].InnerText;
            bool bFound = false;

            //recFindNode(nodes[0], ref bFound);

            return bFound.ToString();
        }

        /*public XmlNode GetTaskSequence(string goalName)
        {
            XmlNodeList nodes = RootNodes;
            XmlNode node = null;
            //_doc.DocumentElement.
            
            //recFindNode(nodes, ref node);

            if (node != null)
            {
                
            }
        }*/

        private void TraverseAllNodes(ref dEachNodeAction nodeAction)
        {
            XmlNodeList nodes = RootNodes;

            foreach (XmlNode node in nodes)
            {
                recTraverseNode(node, ref nodeAction);
            }
        }

        private bool recTraverseNode(XmlNode xmlNode, ref dEachNodeAction nodeAction)
        {
            bool isFinished;

            if (xmlNode.HasChildNodes)
            {
                isFinished = nodeAction(xmlNode, false);

                /*if (myNode.Left != null)
                {
                    recTraverseInorder(myNode.Left, ref myString);
                }

                myString = String.Format("{0}\n{1}", myString, myNode.Key);

                if (myNode.Right != null)
                {
                    recTraverseInorder(myNode.Right, ref myString);
                }*/

                foreach (XmlNode x in xmlNode.ChildNodes)
                {
                    isFinished = recTraverseNode(x, ref nodeAction);

                    if (isFinished)
                    {
                        break;
                    }
                }
            }
            else // is leaf
            {
                isFinished = nodeAction(xmlNode, true);
            }

            return isFinished;

            /*
            if (xmlNode.Attributes["name"].InnerText == "FindAvailableHospital")
            {
                //foundNode = xmlNode;
            }*/
        }

        public string GetAgentFromRole(string roleName)
        {
            XmlNodeList nodes = RootNodes;

            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode n in node.SelectNodes("attribute"))
                {
                    if (n.Attributes["type"].InnerText == "plays" && n.Attributes["name"].InnerText == roleName)
                    {
                        return node.Attributes["name"].InnerText;
                    }
                }

                //temp = node.SelectSingleNode("attribute").Attributes["name"].InnerText;
                //temp = node.SelectSingleNode("arrow").InnerText;
            }

            return String.Format("[{0}]", roleName);
        }
    }
}
