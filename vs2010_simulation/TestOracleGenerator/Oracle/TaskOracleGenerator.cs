﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator.Oracle
{
    public class TaskOracleGenerator
    {
        private TaskModel2 _taskModel;

        public TaskOracleGenerator(string taskModelPath)
        {
            _taskModel = new TaskModel2(taskModelPath);
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

        public TaskModel2 Model
        {
            get
            {
                return _taskModel;
            }
        }
    }
}