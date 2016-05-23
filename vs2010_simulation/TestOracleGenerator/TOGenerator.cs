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
            //sb.AppendFormat("Role to achieve goal: {0}\r\n", foundRole);
            //sb.AppendFormat("Capability:\r\n");

            /*foreach (string s in foundCaps)
            {
                sb.AppendFormat("{0}\r\n", s);
            }

            sb.AppendFormat("\r\n");*/

            /*sb.AppendFormat("Sequence:\r\n");

            for (int i = 0; i < sequence.Length; i++)
            {
                sb.AppendFormat("{0}: {1}.{2}\r\n", sequence[i].index, sequence[i].agent_to, sequence[i].message);
            }*/

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
        public bool Result;
        public int CurrentIndex;
    }

    public class TOGenerator
    {
        string _oracleXMLPath;

        TaskAgentMapper _tAgentMapper;
        TaskOracleGenerator _tOracleGenerator;
        TaskModel _tModel;

        public TOGenerator(string oracleXMLPath)
        {
            _oracleXMLPath = oracleXMLPath;

            _tAgentMapper = new TaskAgentMapper(oracleXMLPath);
            _tOracleGenerator = new TaskOracleGenerator(oracleXMLPath);
            _tModel = new TaskModel(oracleXMLPath);
        }

        public COMPARISON_INFO CompareOutput(string goalName, MessageUnit[] actualOutput, COMPARISON_INFO comparisonInfo)
        {
            TaskNodeTraversalCallback nodeAction;
            bool bCompareResult;

            nodeAction = null;
            comparisonInfo.Result = true;

            // Define lambda callback
            nodeAction = new TaskNodeTraversalCallback((taskNode) =>
            {
                // Found the target goal
                if (taskNode.Name == goalName)
                {
                    comparisonInfo = CompareOutputInternal(actualOutput, taskNode.ChildNodes, comparisonInfo);

                    // Terminate node traversing
                    return false;
                }

                // Continue traversing
                return true;
            });

            _tModel.TraverseTaskNodes(ref nodeAction);

            return comparisonInfo;
        }

        // Analyze single-level tasks in a goal
        private COMPARISON_INFO CompareOutputInternal(MessageUnit[] actualOutput, TaskNodeList taskOracleNodes, COMPARISON_INFO comparisonInfo)
        {
            bool bSubResult;
            //bool bCompareResult;
            MessageUnit msgOracle;
            //int currentIndex;

            bSubResult = false;
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

                    if (node.Type == NODE_TYPE.GOAL)
                    {
                        // Traverse one more time
                        comparisonInfo = CompareOutput(node.Name, actualOutput, comparisonInfo);

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
                            default:
                                comparisonInfo.CurrentIndex = i;
                                break;
                        }
                    }
                    else if (node.Type == NODE_TYPE.TASK)
                    {
                        if (actualOutput[i] == msgOracle)
                        {
                            bSubResult = true;

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
                                default:
                                    comparisonInfo.CurrentIndex = i;
                                    break;
                            }

                            break;
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Undefined node type detected.");
                    }
                }

                comparisonInfo.Result &= bSubResult;
            }

            comparisonInfo.Result &= bSubResult;

            return comparisonInfo;
        }

        /*public bool CompareOutput(string goalName, MessageUnit[] actualOutput, int currentIndex)
        {
            //TreeLogic tl = new TreeLogic(RootNodes[0]);
            TaskModel.EachNodeCallback FUNC_RETRIEVE_TASK;

            bool bFound = false;

            TaskSequenceSet taskSeqSet = new TaskSequenceSet();
            currentIndex = 0;

            // Define task procedure for the node traversal
            FUNC_RETRIEVE_TASK = new TaskModel.EachNodeCallback((xmlNode, isLeaf) =>
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

            return true;
        }*/

        public TestInfo GenerateTestOracle(string goalName)
        {
            TestOracle[] to;
            TestInfo info;

            info = new TestInfo();

            //Console.WriteLine(goalName);
            info.goalName = goalName;

            to = _tOracleGenerator.GenerateTaskSequence(goalName);
            to = _tAgentMapper.GenerateTestOracle(to);

            info.oracle = to;

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

            return _tOracleGenerator.RetrieveGoalList();
        }
    }
}
