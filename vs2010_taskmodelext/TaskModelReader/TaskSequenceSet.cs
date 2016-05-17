using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskModelReader
{
    class TaskSequenceSet
    {
        //List<List<string>> _taskLists;
        TaskNode _previousNode;
        TaskSequence _taskSequence;
        List<TaskSequence> _taskSeqList;

        public TaskSequenceSet()
        {
            _taskSeqList = new List<TaskSequence>();
            _taskSequence = new TaskSequence();
            _previousNode = null;
        }

        public void AddList(TaskSequence taskList)
        {
            _taskSeqList.Add(taskList);
        }

        public void AddSequence(TaskSequenceSet taskSequence, TASK_OPERATOR taskOperator)
        {

        }

        public void AddNode(TaskNode taskNode)
        {
            /*
            // If first node
            if (previousNode == null)
            {
                previousNode = currentNode;
                taskList = new List<string>();
                taskList.Add(currentNode.Name);
            }
            else
            {
                switch (previousNode.Operator)
                {
                    case TASK_OPERATOR.ENABLE:
                        taskList.Add(currentNode.Name);
                        break;
                    case TASK_OPERATOR.CHOICE:
                        taskSeq.AddList(taskList);
                        taskList = new List<string>();
                        taskList.Add(currentNode.Name);
                        break;
                    case TASK_OPERATOR.PARALLEL:
                        break;
                    default:
                        taskList.Add(currentNode.Name);
                        break;
                }

                previousNode = currentNode;
            }
             */
            //_currentSequence.AddTask(nodeName, 

            if (_previousNode == null)
            {
                _taskSequence.AddTask(taskNode);
            }
            else
            {
                switch (_previousNode.Operator)
                {
                    case TASK_OPERATOR.ENABLE:
                        _taskSequence.AddTask(taskNode);
                        break;
                    case TASK_OPERATOR.CHOICE:
                        /*taskSeq.AddList(taskList);
                        taskList = new List<string>();
                        taskList.Add(currentNode.Name);*/
                        _taskSeqList.Add(_taskSequence);
                        _taskSequence = new TaskSequence();
                        _taskSequence.AddTask(taskNode);

                        break;
                    case TASK_OPERATOR.PARALLEL:
                        break;
                    default:
                        _taskSequence.AddTask(taskNode);
                        break;
                }
            }

            // Set current node as the previous node
            _previousNode = taskNode;
        }

        public void Flush()
        {
            _taskSeqList.Add(_taskSequence);
            _taskSequence = new TaskSequence();
        }
    }
}
