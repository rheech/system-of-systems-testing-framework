using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestCaseGenerator.Xml
{
    public enum TRAVERSE_OPTION
    {
        ALL,
        SIBLING_ONLY,
        NONE_FINISH
    }

    class TaskModel : XmlParser
    {
        private List<XmlNode> testCase;
        public delegate TRAVERSE_OPTION dEachNodeAction(XmlNode xmlNode, bool isLeaf);

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

                return TRAVERSE_OPTION.ALL;
            });

            TraverseAllNodes(ref FUNC_RETRIEVE_GOAL);

            return goalList.ToArray();
        }

        public TaskSequenceSet RetrieveTaskSequence(string goalName)
        {
            //TreeLogic tl = new TreeLogic(RootNodes[0]);
            dEachNodeAction FUNC_RETRIEVE_TASK;

            bool bFound = false;

            TaskSequenceSet taskSeqSet = new TaskSequenceSet();
            
            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new dEachNodeAction((xmlNode, isLeaf) =>
            {
                TaskNode taskNode;
                TRAVERSE_OPTION traverseOption;

                taskNode = new TaskNode();
                traverseOption = TRAVERSE_OPTION.ALL;

                // Parse current task node
                taskNode.Name = xmlNode.Attributes["name"].InnerText;

                taskNode.Type = NODE_TYPE.NONE;

                if (xmlNode.Attributes["type"] != null)
                {
                    taskNode.Type = TaskNode.ParseNodeType(xmlNode.Attributes["type"].InnerText);
                }

                taskNode.Operator = TASK_OPERATOR.NONE;

                if (xmlNode.Attributes["operator"] != null)
                {
                    taskNode.Operator = TaskNode.ParseOperator(xmlNode.Attributes["operator"].InnerText);
                }

                taskNode.isLeaf = isLeaf;
                Console.WriteLine(taskNode.Name);

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

            TraverseAllNodes(ref FUNC_RETRIEVE_TASK);

            return taskSeqSet;
        }

        private void TraverseAllNodes(ref dEachNodeAction nodeAction)
        {
            XmlNodeList nodes;
            TRAVERSE_OPTION traverseOption;

            nodes = RootNodes;
            traverseOption = TRAVERSE_OPTION.ALL;

            foreach (XmlNode node in nodes)
            {
                traverseOption = recTraverseNode(node, ref nodeAction);

                if (traverseOption == TRAVERSE_OPTION.NONE_FINISH)
                {
                    break;
                }
            }
        }

        private TRAVERSE_OPTION recTraverseNode(XmlNode xmlNode, ref dEachNodeAction nodeAction)
        {
            bool hasChildNodes;
            TRAVERSE_OPTION traverseOption;

            hasChildNodes = xmlNode.HasChildNodes;
            traverseOption = nodeAction(xmlNode, !hasChildNodes);

            if (hasChildNodes && (traverseOption == TRAVERSE_OPTION.ALL))
            {
                foreach (XmlNode x in xmlNode.ChildNodes)
                {
                    traverseOption = recTraverseNode(x, ref nodeAction);

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
