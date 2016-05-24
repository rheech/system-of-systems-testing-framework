using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOracleGenerator.Xml;

namespace TestOracleGenerator.Oracle
{
    public class TaskAgentMapper
    {
        private RoleModel _roleModel;
        private AgentModel _agentModel;

        public TaskAgentMapper(string oracleXMLPath)
        {
            _roleModel = new RoleModel(oracleXMLPath);
            _agentModel = new AgentModel(oracleXMLPath);
        }

        /*public TaskSequenceSet GetTestSequenceFromTask(TaskSequenceSet taskSequenceSet)
        {
            TaskSequenceSet tSet = new TaskSequenceSet();
            string output;

            for (int i = 0; i < taskSequenceSet.Length; i++)
            {
                for (int j = 0; j < taskSequenceSet[i].Length; j++)
                {
                    
                    //_taskSequence.AddTask(s[j]);
                    //_taskSeqList[i].AddTask(taskSequenceSet[j][k]);
                    //taskSequenceSet[i][j] = "ASDF";

                    output = _roleModel.GetRoleFromMessage(taskSequenceSet[i][j]);
                    output = _agentModel.GetAgentFromRole(output);
                }
            }
            return null;
        }*/

        public MessageUnit GetMessageUnitFromTask(TaskNode taskNode)
        {
            MessageUnit msgUnit;

            msgUnit = new MessageUnit();
            msgUnit.Message = taskNode.Name;

            // Only task node has interaction property
            if (taskNode.Type == NODE_TYPE.TASK)
            {
                _roleModel.GetRoleFromMessage(msgUnit.Message, ref msgUnit.From, ref msgUnit.To);

                msgUnit.From = _agentModel.GetAgentFromRole(msgUnit.From);
                msgUnit.To = _agentModel.GetAgentFromRole(msgUnit.To);
            }

            return msgUnit;
        }
    }
}
