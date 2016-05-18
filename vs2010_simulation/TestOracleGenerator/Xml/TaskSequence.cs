using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOracleGenerator.Xml
{
    public class TaskSequence
    {
        List<string> _taskList;

        public TaskSequence()
        {
            _taskList = new List<string>();
        }

        public TaskSequence(TaskSequence seq)
        {
            _taskList = new List<string>(seq._taskList);
        }

        public void AddTask(string taskName)
        {
            _taskList.Add(taskName);
        }

        public void AddTask(TaskNode taskNode)
        {
            AddTask(taskNode.Name);
        }

        public int Length
        {
            get
            {
                if (_taskList != null)
                {
                    return _taskList.Count;
                }

                return 0;
            }
        }

        public string this[int index]
        {
            get
            {
                if (_taskList != null)
                {
                    return _taskList[index];
                }

                throw new Exception("Invalid index");
            }
            set
            {
                if (_taskList != null)
                {
                    _taskList[index] = value;
                }

                throw new Exception("Invalid index");
            }
        }
    }
}
