using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCaseGenerator.Xml;

namespace TestCaseGenerator.Oracle
{
    public class TaskAgentMapper
    {
        private RoleModel _roleModel;

        public TaskAgentMapper()
        {
            //_roleModel = new RoleModel();
        }

        public TaskSequenceSet GetTestSequenceFromTask(TaskSequenceSet taskSequenceSet)
        {
            TaskSequenceSet tSet = new TaskSequenceSet();

            for (int i = 0; i < taskSequenceSet.Length; i++)
            {
                for (int j = 0; j < taskSequenceSet[i].Length; j++)
                {
                    //_taskSequence.AddTask(s[j]);
                    //_taskSeqList[i].AddTask(taskSequenceSet[j][k]);
                    //taskSequenceSet[i][j] = "ASDF";
                    
                    //GetRoleFromMessage(taskSequenceSet[i][j]);
                }
            }
            return null;
        }
    }
}
