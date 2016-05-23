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

        public MessageUnit GetMessageUnitFromTask(string messageTask)
        {
            MessageUnit msgUnit;

            msgUnit = new MessageUnit();
            msgUnit.Message = messageTask;

            _roleModel.GetRoleFromMessage(msgUnit.Message, ref msgUnit.From, ref msgUnit.To);

            msgUnit.From = _agentModel.GetAgentFromRole(msgUnit.From);
            msgUnit.To = _agentModel.GetAgentFromRole(msgUnit.To);

            return msgUnit;
        }

        public TestOracle[] GenerateTestOracle(TestOracle[] abstractOracle)
        {
            List<TestOracle> tOracleList;
            MessageUnit tempMsgUnit;

            tOracleList = new List<TestOracle>(abstractOracle);

            for (int i = 0; i < tOracleList.Count; i++)
            {
                for (int j = 0; j < tOracleList[i].Length; j++)
                {
                    tempMsgUnit = tOracleList[i][j];

                    if (_roleModel.GetRoleFromMessage(tempMsgUnit.Message, ref tempMsgUnit.From, ref tempMsgUnit.To))
                    {
                        tempMsgUnit.From = _agentModel.GetAgentFromRole(tempMsgUnit.From);
                        tempMsgUnit.To = _agentModel.GetAgentFromRole(tempMsgUnit.To);
                    }
                    else
                    {
                        throw new ApplicationException("Error occurred while processing role model.");
                    }
                }
            }

            return tOracleList.ToArray();
        }
    }
}
