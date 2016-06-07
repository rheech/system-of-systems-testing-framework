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

    public abstract class OracleGenerator
    {
        string _oracleXMLPath;

        protected TaskAgentMapper _tAgentMapper;
        protected TaskModel _tModel;

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
