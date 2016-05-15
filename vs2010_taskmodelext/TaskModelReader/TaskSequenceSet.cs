using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class TaskSequenceSet
    {
        List<List<string>> _taskLists;

        public TaskSequenceSet()
        {
            _taskLists = new List<List<string>>();
        }

        public void AddList(List<string> taskList)
        {
            _taskLists.Add(taskList);
        }

        public void AppendSequence(TaskSequenceSet taskSequence, TASK_OPERATOR taskOperator)
        {

        }
    }
}
