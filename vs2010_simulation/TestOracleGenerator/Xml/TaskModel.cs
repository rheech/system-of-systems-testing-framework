using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public delegate void TaskNodeTraversalCallback(TaskNode taskNode);

    public class TaskModel : XmlParser
    {
        private List<XmlNode> testCase;
        public delegate TRAVERSE_OPTION EachNodeCallback(XmlNode xmlNode, bool isLeaf);

        public TaskModel(string file)
            : base(file, "/SoS/TaskModel/node")
        {
            testCase = new List<XmlNode>();
        }

        public string[] RetrieveGoalList()
        {
            EachNodeCallback FUNC_RETRIEVE_GOAL;
            List<string> goalList = new List<string>();

            // Define goal procedure for the node traversal
            FUNC_RETRIEVE_GOAL = new EachNodeCallback((xmlNode, isLeaf) =>
            {
                //Console.WriteLine(xmlNode.Attributes["name"].InnerText);

                if (xmlNode.Attributes["type"].InnerText == "goal")
                {
                    goalList.Add(xmlNode.Attributes["name"].InnerText);
                }

                return TRAVERSE_OPTION.ALL;
            });

            TraverseAllXmlNodes(ref FUNC_RETRIEVE_GOAL);

            return goalList.ToArray();
        }

        public void TraverseTaskNodes(ref TaskNodeTraversalCallback cbNodeAction)
        {
            TaskNode tNode;

            tNode = new TaskNode(RootNodes[0]);

            tNode.TraverseChildNodes(ref cbNodeAction);
        }

        /*public TaskNode FindGoal(string goalName)
        {

        }*/

        /*
        private TRAVERSE_OPTION recTraverseTaskModel(XmlNode xmlNode, ref EachTaskNodeCallback cbNodeAction)
        {
            bool hasChildNodes;
            TRAVERSE_OPTION traverseOption;
            TaskNode taskNode;

            // Convert to TaskNode
            taskNode = new TaskNode();
            taskNode.xmlNode = xmlNode;

            hasChildNodes = xmlNode.HasChildNodes;
            traverseOption = cbNodeAction(xmlNode, !hasChildNodes);

            if (hasChildNodes && (traverseOption == TRAVERSE_OPTION.ALL))
            {
                foreach (XmlNode x in xmlNode.ChildNodes)
                {
                    traverseOption = recTraverseTaskModel(x, ref cbNodeAction);

                    if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                    {
                        break;
                    }
                }
            }

            return traverseOption;
        }*/

        // Depreciated
        public void TraverseAllXmlNodes(ref EachNodeCallback nodeAction)
        {
            XmlNodeList nodes;
            TRAVERSE_OPTION traverseOption;

            nodes = RootNodes;
            traverseOption = TRAVERSE_OPTION.ALL;

            foreach (XmlNode node in nodes)
            {
                traverseOption = recTraverseXmlNode(node, ref nodeAction);

                if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                {
                    break;
                }
            }
        }

        private TRAVERSE_OPTION recTraverseXmlNode(XmlNode xmlNode, ref EachNodeCallback nodeAction)
        {
            bool hasChildNodes;
            TRAVERSE_OPTION traverseOption;

            hasChildNodes = xmlNode.HasChildNodes;
            traverseOption = nodeAction(xmlNode, !hasChildNodes);

            if (hasChildNodes && (traverseOption == TRAVERSE_OPTION.ALL))
            {
                foreach (XmlNode x in xmlNode.ChildNodes)
                {
                    traverseOption = recTraverseXmlNode(x, ref nodeAction);

                    if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                    {
                        break;
                    }
                }
            }

            return traverseOption;
        }
    }
}
