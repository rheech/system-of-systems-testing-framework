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

    public class TaskModel2 : XmlParser
    {
        private List<XmlNode> testCase;
        public delegate TRAVERSE_OPTION EachNodeCallback(XmlNode xmlNode, bool isLeaf);

        public TaskModel2(string file)
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

        public TaskSequenceSet RetrieveTaskSequence(string goalName)
        {
            //TreeLogic tl = new TreeLogic(RootNodes[0]);
            EachNodeCallback FUNC_RETRIEVE_TASK;

            bool bFound = false;

            TaskSequenceSet taskSeqSet = new TaskSequenceSet();
            
            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new EachNodeCallback((xmlNode, isLeaf) =>
            {
                TaskNode2 taskNode;
                TRAVERSE_OPTION traverseOption;

                taskNode = new TaskNode2();
                traverseOption = TRAVERSE_OPTION.ALL;

                // Parse current task node
                taskNode.Name = xmlNode.Attributes["name"].InnerText;

                taskNode.Type = NODE_TYPE.NONE;

                if (xmlNode.Attributes["type"] != null)
                {
                    taskNode.Type = TaskNode2.ParseNodeType(xmlNode.Attributes["type"].InnerText);
                }

                taskNode.Operator = TASK_OPERATOR.NONE;

                if (xmlNode.Attributes["operator"] != null)
                {
                    taskNode.Operator = TaskNode2.ParseOperator(xmlNode.Attributes["operator"].InnerText);
                }

                taskNode.isLeaf = isLeaf;
                //Console.WriteLine(taskNode.Name);

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
                    if (!taskNode.hasNextNode)
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

        /*public void TraverseTaskModel(ref EachTaskNodeCallback cbNodeAction)
        {
            XmlNodeList nodes;
            TRAVERSE_OPTION traverseOption;

            nodes = RootNodes;
            traverseOption = TRAVERSE_OPTION.ALL;

            foreach (XmlNode node in nodes)
            {
                traverseOption = recTraverseTaskModel(node, ref cbNodeAction);

                if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                {
                    break;
                }
            }
        }

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
