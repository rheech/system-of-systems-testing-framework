using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class TaskSequence
    {
        List<string> _taskList;

        public TaskSequence()
        {
            _taskList = new List<string>();
        }

        public void AddTask(string taskName)
        {
            _taskList.Add(taskName);
        }

        public void AddTask(TaskNode taskNode)
        {
            AddTask(taskNode.Name);
        }
    }
}
