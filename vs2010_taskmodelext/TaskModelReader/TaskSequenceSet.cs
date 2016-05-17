using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class TaskSequenceSet
    {
        //List<List<string>> _taskLists;
        List<TaskSequence> _taskLists;

        public TaskSequenceSet()
        {
            _taskLists = new List<TaskSequence>();
        }

        public void AddList(TaskSequence taskList)
        {
            _taskLists.Add(taskList);
        }

        public void AppendSequence(TaskSequenceSet taskSequence, TASK_OPERATOR taskOperator)
        {

        }

        public void AddNode(TaskNode node)
        {
        }


    }
}
