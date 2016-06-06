using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;
using TestOracleGenerator.Oracle;
using BugTracker;

namespace TestOracleGenerator
{
    public struct Arrow
    {
        public int index;
        public string agent_to;
        public string message;
        public string role_to;

        public override string ToString()
        {
            return String.Format("{0}.{1}", agent_to, message);
        }
    }

    public struct COMPARISON_INFO
    {
        public string GoalName;
        public bool Result;
        public int ResultCount;
        public int CurrentIndex;
    }

    public class OracleGenerator
    {
        string _oracleXMLPath;

        TaskAgentMapper _tAgentMapper;
        TaskModel _tModel;

        public OracleGenerator(string oracleXMLPath)
        {
            _oracleXMLPath = oracleXMLPath;

            _tAgentMapper = new TaskAgentMapper(oracleXMLPath);
            _tModel = new TaskModel(oracleXMLPath);
        }

        private TestOracleSet GenerateTestOracleSet()
        {
            TaskNodeList goalList;
            TestOracleSet oracleSet;
            
            //goalList = RetrieveGoalList();
            goalList = RetrieveAllGoalNodes();
            oracleSet = new TestOracleSet();

            foreach (TaskNode n in goalList)
            {
                oracleSet.Add(GenerateTestOracleByGoal(n));
            }

            return oracleSet;
        }

        #region Testing
        public bool CompareOutput(string goalName, MessageUnitList actualOutput)
        {
            COMPARISON_INFO comparisonInfo;
            comparisonInfo = new COMPARISON_INFO();
            comparisonInfo.GoalName = goalName;
            comparisonInfo.Result = true;
            comparisonInfo.CurrentIndex = 0;

            comparisonInfo = CompareOutputInternal(goalName, actualOutput, comparisonInfo);

            return comparisonInfo.Result;
        }

        private COMPARISON_INFO CompareOutputInternal(string goalName, MessageUnitList actualOutput, COMPARISON_INFO comparisonInfo)
        {
            TaskNodeTraversalCallback nodeAction;
            bool bFoundGoal;

            nodeAction = null;
            bFoundGoal = false;
            comparisonInfo.Result = true;

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found the target goal
                if (taskNode.Name == goalName)
                {
                    bFoundGoal = true;
                    comparisonInfo = recCompareOutputInternal(actualOutput, taskNode.ChildNodes, comparisonInfo);

                    // Terminate node traversing
                    return false;
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            // if goal is not found, throw exception
            if (!bFoundGoal)
            {
                throw new ApplicationException(String.Format("Goal ({0}) is not found.", goalName));
            }

            return comparisonInfo;
        }

        // Analyze single-level tasks in a specific goal (after found goal)
        private COMPARISON_INFO recCompareOutputInternal(MessageUnitList actualOutput, TaskNodeList taskOracleNodes, COMPARISON_INFO comparisonInfo)
        {
            bool bSubResult;
            int iSubResultCount;
            bool bBreakLoop;
            //bool bCompareResult;
            MessageUnit msgOracle;
            COMPARISON_INFO tempInfo;
            //int currentIndex;

            bSubResult = false;
            bBreakLoop = false;
            tempInfo = new COMPARISON_INFO();
            //currentIndex = 0;

            // Traverse child nodes
            foreach (TaskNode node in taskOracleNodes)
            {
                // Convert task to message
                msgOracle = _tAgentMapper.GetMessageUnitFromTask(node);

                iSubResultCount = 0;

                // if there is a nested goal in the sequence
                //bFoundEntry = CompareOutputFromMessage(actualOutput, msgOracle, iPointer);
                // Compare output with oracle
                for (int i = comparisonInfo.CurrentIndex; i < actualOutput.Count; i++)
                {
                    bSubResult = false;

                    // Undefined node type exception
                    if (node.Type == NODE_TYPE.NONE)
                    {
                        throw new ApplicationException("Undefined node type detected.");
                    }

                    // If oracle matches with a task name
                    if (actualOutput[i] == msgOracle)
                    {
                        //bSubResult = true;
                        //iSubResultCount++;

                        //iSubResultCount = actualOutput.OccurrenceOf(msgOracle);
                        /*
                        for (int k = i + 1; k < actualOutput.Count; k++)
                        {
                            if (actualOutput[k] == msgOracle)
                            {
                                iSubResultCount++;
                            }
                        }*/

                        //if (iSubResultCount == node.RecursionCount)
                        {
                            bSubResult = true;
                        }
                    }
                    else if (node.Type == NODE_TYPE.GOAL) // abstract task
                    {
                        tempInfo = CompareOutputInternal(node.Name, actualOutput, comparisonInfo);

                        bSubResult = tempInfo.Result;
                        // comparisonInfo.Result &= tempInfo.Result;
                        // comparisonInfo.CurrentIndex = tempInfo.CurrentIndex;
                    }

                    // Setup next task
                    if (bSubResult)
                    {
                        // Define next index by checking the operator
                        switch (node.Operator)
                        {
                            case TASK_OPERATOR.INTERLEAVING: // ORDER INDEPENDENT + CHOICE
                                comparisonInfo.CurrentIndex = 0;
                                
                                comparisonInfo.CurrentIndex = actualOutput.Count; // Exit for

                                if (comparisonInfo.Result)
                                {
                                    return comparisonInfo;
                                }
                                else
                                {
                                    // reset for the next optional comparison
                                    comparisonInfo.Result = true;
                                }
                                break;
                            case TASK_OPERATOR.ORDER_INDEPENDENT:
                                comparisonInfo.CurrentIndex = 0;
                                break;
                            case TASK_OPERATOR.CHOICE:
                                comparisonInfo.CurrentIndex = actualOutput.Count; // Exit for

                                if (comparisonInfo.Result)
                                {
                                    return comparisonInfo;
                                }
                                else
                                {
                                    // reset for the next optional comparison
                                    comparisonInfo.Result = true;
                                }

                                break;
                            case TASK_OPERATOR.SEQUENTIAL:
                                comparisonInfo.CurrentIndex = i;
                                break;
                            case TASK_OPERATOR.NONE:
                                bBreakLoop = true;
                                break;
                            default:
                                throw new ApplicationException("Undefined operator detected.");
                        }

                        // exit for-loop
                        break;
                    }
                }

                comparisonInfo.Result &= bSubResult;

                if (bBreakLoop)
                {
                    break;
                }
            }

            comparisonInfo.Result &= bSubResult;

            return comparisonInfo;
        }
        #endregion

        private TaskNodeList RetrieveAllGoalNodes()
        {
            TaskNodeTraversalCallback nodeAction;
            TaskNodeList goalList;

            nodeAction = null;
            goalList = new TaskNodeList();

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found a goal
                if (taskNode.Type == NODE_TYPE.GOAL)
                {
                    goalList.Add(taskNode);
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            return goalList;
        }

        private TestOracle GenerateTestOracleByGoal(TaskNode goalNode)
        {
            TaskNodeTraversalCallback nodeAction;
            TaskNode goalTaskNode;
            MessageUnitList messagesList;

            goalTaskNode = null;
            messagesList = new MessageUnitList();

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found the target goal
                if (taskNode.IsSameContent(goalNode))
                {
                    if (taskNode.HasChildNodes)
                    {
                        goalTaskNode = taskNode;

                        foreach (TaskNode node in taskNode.ChildNodes)
                        {
                            messagesList.Add(_tAgentMapper.GetMessageUnitFromTask(node));
                        }
                    }

                    // Terminate node traversing
                    return false;
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            // if error
            if (goalTaskNode == null)
            {
                throw new ApplicationException("Task node list cannot be null.");
            }

            return new TestOracle(goalTaskNode, messagesList);
        }

        public TestOracle GenTestOracle(string goalName)
        {
            TaskNodeTraversalCallback nodeAction;
            TaskNode goalTaskNode;
            MessageUnitList messagesList;

            goalTaskNode = null;
            messagesList = new MessageUnitList();

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found the target goal
                if (taskNode.Name == goalName)
                {
                    if (taskNode.HasChildNodes)
                    {
                        goalTaskNode = taskNode;

                        foreach (TaskNode node in taskNode.ChildNodes)
                        {
                            messagesList.Add(_tAgentMapper.GetMessageUnitFromTask(node));
                        }
                    }

                    // Terminate node traversing
                    return false;
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            // if error
            if (goalTaskNode == null)
            {
                throw new ApplicationException("Task node list cannot be null.");
            }

            return new TestOracle(goalTaskNode, messagesList);
        }

        public string[] RetrieveGoalList()
        {
            TaskNodeTraversalCallback nodeAction;
            List<string> goalList;

            nodeAction = null;
            goalList = new List<string>();

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found a goal
                if (taskNode.Type == NODE_TYPE.GOAL)
                {
                    goalList.Add(taskNode.Name);
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            return goalList.ToArray();
        }
    }
}
