using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class TaskSequence
    {
        List<List<string>> _taskLists;

        public TaskSequence()
        {
            _taskLists = new List<List<string>>();
        }

        public void AddList(List<string> taskList)
        {
            _taskLists.Add(taskList);
        }
    }
}
