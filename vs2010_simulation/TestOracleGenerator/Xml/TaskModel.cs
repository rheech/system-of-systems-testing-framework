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

        public TaskSequenceSet RetrieveTaskSequence(string goalName)
        {
            //TreeLogic tl = new TreeLogic(RootNodes[0]);
            EachNodeCallback FUNC_RETRIEVE_TASK;

            bool bFound = false;

            TaskSequenceSet taskSeqSet = new TaskSequenceSet();

            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new EachNodeCallback((xmlNode, isLeaf) =>
            {
                TaskNode taskNode;
                TRAVERSE_OPTION traverseOption;

                taskNode = new TaskNode(xmlNode);
                traverseOption = TRAVERSE_OPTION.ALL;

                // Found Goal
                if (taskNode.Name == goalName)
                {
                    bFound = true;
                    //previousNode = null;
                    traverseOption = TRAVERSE_OPTION.ALL;
                }
                else if (bFound) // after goal has found
                {
                    // if a goal node in the goal
                    if (taskNode.Type == NODE_TYPE.GOAL)
                    {
                        TaskSequenceSet seq;
                        seq = RetrieveTaskSequence(taskNode.Name);

                        taskSeqSet.AddSequence(seq);

                        //taskList.Add(currentNode.Name);

                        traverseOption = TRAVERSE_OPTION.SIBLING_ONLY;
                    }
                    else // task traversal
                    {
                        taskSeqSet.AddNode(taskNode);
                    }

                    // if last node
                    if (!taskNode.HasChildNodes)
                    {
                        //taskSeq.AddList(taskList);
                        //taskSeqSet.Flush();
                        traverseOption = TRAVERSE_OPTION.NONE_FINISH;
                    }
                }

                return traverseOption;
            });

            TraverseAllXmlNodes(ref FUNC_RETRIEVE_TASK);

            taskSeqSet.Flush();

            return taskSeqSet;
        }
    }
}
