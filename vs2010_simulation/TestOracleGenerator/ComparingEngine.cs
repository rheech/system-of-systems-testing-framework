﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Oracle;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator
{
    public enum COMPARISON_LEVEL
    {
        LEVEL1,
        LEVEL2,
        LEVEL3
    };

    public class ComparingEngine : OracleGenerator
    {
        public ComparingEngine(string oracleXMLPath)
            : base(oracleXMLPath)
        {
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
                        switch (ComparisonLevel)
                        {
                            case COMPARISON_LEVEL.LEVEL1:
                                bSubResult = true;
                                break;
                            case COMPARISON_LEVEL.LEVEL2:
                                if (node.RecursionCount != -1)
                                {
                                    iSubResultCount++;
                                }
                                else
                                {
                                    bSubResult = true;
                                }
                                break;
                            case COMPARISON_LEVEL.LEVEL3:
                                if (node.RecursionCount != -1)
                                {
                                    iSubResultCount = actualOutput.OccurrenceOf(msgOracle);
                                }
                                else
                                {
                                    bSubResult = true;
                                }
                                break;
                            default:
                                break;
                        }

                        if (!bSubResult && 
                                iSubResultCount == node.RecursionCount)
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
                            case TASK_OPERATOR.INTERLEAVING: // ORDER INDEPENDENT + CHOICE + CONCURRENCY
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
                            case TASK_OPERATOR.SYNCHRONIZATION: // CONCURRENCY with Information Passing
                                if (actualOutput[i].Parameter.Length > 0 &&
                                        actualOutput[i].Cycle == actualOutput[i + 1].Cycle)
                                {
                                    bSubResult = true;
                                }
                                break;
                            case TASK_OPERATOR.CONCURRENCY: // CONCURRENCY
                                if (actualOutput[i].Cycle == actualOutput[i + 1].Cycle)
                                {
                                    bSubResult = true;
                                }
                                break;
                            case TASK_OPERATOR.ENABLE:
                                comparisonInfo.CurrentIndex = i;
                                break;
                            case TASK_OPERATOR.ENABLEINFO:
                                if (actualOutput[i].Parameter.Length > 0)
                                {
                                    comparisonInfo.CurrentIndex = i;
                                }

                                break;
                            case TASK_OPERATOR.ORDER_INDEPENDENT:
                                comparisonInfo.CurrentIndex = 0;
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

        public COMPARISON_LEVEL ComparisonLevel
        {
            get;
            set;
        }
    }
}
