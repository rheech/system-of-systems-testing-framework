using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestOracleGenerator.Xml
{
    public class TaskSequence : TaskInterface
    {
        List<TaskNode> _taskList;

        public TaskSequence()
        {
            _taskList = new List<TaskNode>();
        }

        public TaskSequence(TaskSequence seq)
        {
            _taskList = new List<TaskNode>(seq._taskList);
        }

        /*private void AddTask(string taskName)
        {
            _taskList.Add(taskName);
        }*/

        public void AddTask(TaskNode taskNode)
        {
            _taskList.Add(taskNode);
            //AddTask(taskNode.Name);
        }

        public TASK_OPERATOR Operator
        {
            get
            {
                if (_taskList != null)
                {
                    return _taskList[_taskList.Count - 1].Operator;
                }

                return TASK_OPERATOR.NONE;
            }
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

        public TaskNode this[int index]
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
