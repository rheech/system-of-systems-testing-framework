using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;
using TestOracleGenerator.Oracle;

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

    public struct TestInfo
    {
        public string goalName;
        //public string foundRole;
        //public string[] foundCaps;
        //public Arrow[] sequence;
        public TestOracle[] oracle;

        public override string ToString()
        {
            string output;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Goal: {0}\r\n", goalName);
            sb.AppendFormat("\r\n");
            sb.AppendFormat("Sequence:\r\n");

            foreach (TestOracle o in oracle)
            {
                sb.AppendFormat("{0}\r\n", o);
            }

            try
            {
                output = sb.ToString();
            }
            catch (Exception)
            {
                output = base.ToString();
            }

            return output;
        }
    }

    public struct COMPARISON_INFO
    {
        public string GoalName;
        public bool Result;
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

        public bool CompareOutput(string goalName, MessageUnit[] actualOutput)
        {
            COMPARISON_INFO comparisonInfo;
            comparisonInfo = new COMPARISON_INFO();
            comparisonInfo.GoalName = goalName;
            comparisonInfo.Result = true;
            comparisonInfo.CurrentIndex = 0;

            comparisonInfo = CompareOutputInternal(goalName, actualOutput, comparisonInfo);

            return comparisonInfo.Result;
        }

        private COMPARISON_INFO CompareOutputInternal(string goalName, MessageUnit[] actualOutput, COMPARISON_INFO comparisonInfo)
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

        // Analyze single-level tasks in a goal
        private COMPARISON_INFO recCompareOutputInternal(MessageUnit[] actualOutput, TaskNodeList taskOracleNodes, COMPARISON_INFO comparisonInfo)
        {
            bool bSubResult;
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
                msgOracle = _tAgentMapper.GetMessageUnitFromTask(node.Name);

                // if there is a nested goal in the sequence
                //bFoundEntry = CompareOutputFromMessage(actualOutput, msgOracle, iPointer);
                // Compare output with oracle
                for (int i = comparisonInfo.CurrentIndex; i < actualOutput.Length; i++)
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
                        bSubResult = true;
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
                            case TASK_OPERATOR.ENABLE:
                                comparisonInfo.CurrentIndex = i;
                                break;
                            case TASK_OPERATOR.CHOICE:
                                comparisonInfo.CurrentIndex = actualOutput.Length; // Exit for

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
                            case TASK_OPERATOR.PARALLEL: // NOT IMPLEMENTED
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

        public TestInfo GenerateTestOracle(string goalName)
        {
            TestOracle[] to;
            TestInfo info;

            info = new TestInfo();

            //Console.WriteLine(goalName);
            info.goalName = goalName;

            to = TestOracle.FromTaskSequenceSet(_tModel.RetrieveTaskSequence(goalName));
            to = _tAgentMapper.GenerateTestOracle(to);

            //info.oracle = to;

            return info;

            /*RoleModel role = new RoleModel(RoleModel);
            AgentModel agent = new AgentModel(AgentModel);
            TaskModel task = new TaskModel(TaskModel);

            TestInfo info = new TestInfo();
            info.goalName = goalName;
            /*info.foundRole = role.GetRoleFromGoal(goalName);
            info.foundCaps = role.GetCapabilityFromRole(info.foundRole);
            info.sequence = protocol.TrackSequence(info.foundRole, agent);* /

            TaskSequenceSet tSet;
            tSet = task.RetrieveTaskSequence(goalName);

            //agent.GetAgentFromRole("BaseAmbulance");

            //role.GetTestSequenceFromTask(tSet);
            
            return info;

            /*StringBuilder sb = new StringBuilder();

            string foundRole = role.GetRoleFromGoal(goalName);
            string[] foundCaps = role.GetCapabilityFromRole(foundRole);

            sb.AppendFormat("Goal: {0}\r\n", goalName);
            sb.AppendFormat("Role to achieve goal: {0}\r\n", foundRole);
            sb.AppendFormat("Capability:\r\n");

            foreach (string s in foundCaps)
            {
                sb.AppendFormat("{0}\r\n", s);
            }

            sb.AppendFormat("\r\n");

            Arrow[] arr = protocol.TrackSequence(foundRole);

            sb.AppendFormat("Sequence:\r\n");

            for (int i = 0; i < arr.Length; i++)
            {
                sb.AppendFormat("{0}: {1}.{2}\r\n", arr[i].index, agent.GetAgentFromRole(arr[i].to), arr[i].name);
            }

            return sb.ToString();*/
        }

        public string[] RetrieveGoalList()
        {
            //string[] goalList = { "Communicate", "Triage", "Treatment", "MedComm", "Transportation" };
            /*string[] goalList = { "SavePatient" };

            // Special treatment for MCI Scenario (Temporary)
            if (_oracleXMLPath.Contains("Scenario_MCI.xml"))
            {
                return goalList;
            }*/

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
