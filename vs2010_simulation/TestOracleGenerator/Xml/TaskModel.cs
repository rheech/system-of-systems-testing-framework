using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestOracleGenerator.Xml
{
    public enum TRAVERSE_OPTION
    {
        ALL,
        SIBLING_ONLY,
        NONE_FINISH
    }

    public delegate bool TaskNodeTraversalCallback(TaskNode taskNode);

    public class TaskModel : XmlParser
    {
        private List<XmlNode> testCase;
        public delegate TRAVERSE_OPTION EachNodeCallback(XmlNode xmlNode, bool isLeaf);

        public TaskModel(string file)
            : base(file, "/SoS/TaskModel")
        {
            testCase = new List<XmlNode>();
        }

        public void TraverseTaskNodes(ref TaskNodeTraversalCallback cbNodeAction)
        {
            TaskNode tNode;

            tNode = new TaskNode(RootNodes[0]);

            tNode.TraverseChildNodes(ref cbNodeAction);
        }

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
