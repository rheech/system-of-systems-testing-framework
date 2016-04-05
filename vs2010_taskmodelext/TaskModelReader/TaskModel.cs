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
        public delegate void dEachNodeAction(XmlNode xmlNode);

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
            FUNC_RETRIEVE_GOAL = new dEachNodeAction((xmlNode) =>
            {
                //Console.WriteLine(xmlNode.Attributes["name"].InnerText);

                if (xmlNode.Attributes["type"].InnerText == "goal")
                {
                    goalList.Add(xmlNode.Attributes["name"].InnerText);
                }
            });

            TraverseAllNodes(ref FUNC_RETRIEVE_GOAL);

            return goalList.ToArray();
        }

        public void RetrieveTaskSequence(string goalName)
        {
            TaskSequence priorTasks = new TaskSequence();
            dEachNodeAction FUNC_RETRIEVE_TASK;
            
            bool bFound = false;

            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new dEachNodeAction((xmlNode) =>
            {
                Console.WriteLine(xmlNode.Attributes["name"].InnerText);

                if (xmlNode.Attributes["name"].InnerText == goalName)
                {
                    bFound = true;
                    //goalList.Add(xmlNode.Attributes["name"].InnerText);
                }

                if (xmlNode.Attributes["operator"] != null)
                {
                    switch (xmlNode.Attributes["operator"].InnerText)
                    {
                        case "enable":
                            priorTasks.Enqueue(xmlNode);
                            break;
                        case "choice":
                            priorTasks.Duplicate();
                            break;
                        default:
                            break;
                    }
                }

                if (bFound)
                {

                }
            });

            TraverseAllNodes(ref FUNC_RETRIEVE_TASK);
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

        private void recTraverseNode(XmlNode xmlNode, ref dEachNodeAction nodeAction)
        {
            nodeAction(xmlNode);

            if (xmlNode.HasChildNodes)
            {
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
                    recTraverseNode(x, ref nodeAction);
                }
            }
            else // is leaf
            {

            }

            if (xmlNode.Attributes["name"].InnerText == "FindAvailableHospital")
            {
                //foundNode = xmlNode;
            }
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
