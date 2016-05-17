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

        private void DuplicateSequence(int numOfDuplicates)
        {
            List<TaskSequence> newSequence;

            newSequence = new List<TaskSequence>();

            foreach (TaskSequence seq in _taskSeqList)
            {
                for (int i = 0; i < numOfDuplicates; i++)
                {
                    newSequence.Add(new TaskSequence(seq));
                }
            }

            _taskSeqList = newSequence;
        }

        public TaskSequence[] RetrieveAllTaskSequence()
        {
            return _taskSeqList.ToArray();
        }

        // return true if has next node
        public void AddSequence(TaskSequenceSet taskSeqSet)
        {
            TaskSequence[] seqList = taskSeqSet.RetrieveAllTaskSequence();

            // if first node
            if (_previousNode == null)
            {
                // Just copy everything from the subtask (goal)
                _taskSeqList = new List<TaskSequence>(seqList);
                _previousNode = taskSeqSet._previousNode;
                _taskSequence = taskSeqSet._taskSequence;
            }
            else
            {
                // Flush all previous nodes
                Flush();

                // duplicate sequence if necessary
                if (taskSeqSet.Length > 1)
                {
                    DuplicateSequence(taskSeqSet.Length);
                }

                switch (_previousNode.Operator)
                {
                    case TASK_OPERATOR.ENABLE:
                        for (int i = 0; i < _taskSeqList.Count; i++)
                        {
                            for (int j = 0; j < taskSeqSet.Length; j++)
                            {
                                for (int k = 0; k < taskSeqSet[j].Length; k++)
                                {
                                    //_taskSequence.AddTask(s[j]);
                                    _taskSeqList[i].AddTask(taskSeqSet[j][k]);
                                }

                                i++;
                            }
                        }
                        
                        break;
                    case TASK_OPERATOR.CHOICE:
                        // ** NOT IMPLEMENTED **
                        //_taskSeqList.Add(_taskSequence);
                        //_taskSequence = new TaskSequence();
                        //_taskSequence.AddTask(taskNode);
                        break;
                    case TASK_OPERATOR.PARALLEL:
                        // ** NOT IMPLEMENTED **
                        /*for (int i = 0; i < _taskSeqList.Count; i++)
                        {
                            for (int j = 0; j < taskSeqSet.Length; j++)
                            {
                                for (int k = 0; k < taskSeqSet[j].Length; k++)
                                {
                                    //_taskSequence.AddTask(s[j]);
                                    _taskSeqList[i].AddTask(taskSeqSet[j][k]);
                                }

                                i++;
                            }
                        }
                        */
                        break;
                    default:
                        // ** NOT IMPLEMENTED **
                        //_taskSequence.AddTask(taskNode);
                        break;
                }
            }

            // Set current node as the previous node
            //_previousNode = taskNode;
            _previousNode = taskSeqSet._previousNode;
        }

        // return true if has next node
        public void AddNode(TaskNode taskNode)
        {
            // if first node
            if (_previousNode == null)
            {
                if (_taskSequence == null)
                {
                    _taskSequence = new TaskSequence();
                }

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

            // if last node
            if (!taskNode.hasNextNode)
            {
                //taskSeq.AddList(taskList);
                //taskSeqSet.Flush();
                _taskSeqList.Add(_taskSequence);
                //traverseOption = TRAVERSE_OPTION.NONE_FINISH;
            }

            // Set current node as the previous node
            _previousNode = taskNode;
        }

        public int Length
        {
            get
            {
                if (_taskSeqList != null)
                {
                    return _taskSeqList.Count;
                }

                return 0;
            }
        }

        public TaskSequence this[int index]
        {
            get
            {
                if (_taskSeqList != null)
                {
                    return _taskSeqList[index];
                }

                throw new Exception("Invalid index");
            }
        }

        public void Flush()
        {
            _taskSeqList.Add(_taskSequence);
            _taskSequence = null;
        }
    }
}
