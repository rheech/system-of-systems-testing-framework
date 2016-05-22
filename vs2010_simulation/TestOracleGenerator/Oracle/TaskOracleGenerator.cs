using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator.Oracle
{
    public class TaskOracleGenerator
    {
        private TaskModel _taskModel;

        public TaskOracleGenerator(string taskModelPath)
        {
            _taskModel = new TaskModel(taskModelPath);
        }

        public TestOracle[] GenerateTaskSequence(string goalName)
        {
            TaskSequenceSet tSet;

            tSet = _taskModel.RetrieveTaskSequence(goalName);

            return TestOracle.FromTaskSequenceSet(tSet);
        }

        public string[] RetrieveGoalList()
        {
            return _taskModel.RetrieveGoalList();
        }
    }
}
