using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCaseGenerator.Xml;

namespace TestCaseGenerator
{
    public class TCGenerator
    {
        private string _goalPath;
        private string _protocolPath;
        private string _rolePath;
        private string _agentPath;

        public TCGenerator()
        {

        }

        public string GenerateTestCase(string goalName)
        {
            XmlRoleModel role = new XmlRoleModel(RoleModel);
            XmlProtocolModel protocol = new XmlProtocolModel(ProtocolModel);
            XmlAgentModel agent = new XmlAgentModel(AgentModel);

            StringBuilder sb = new StringBuilder();

            string foundRole = role.GetRoleFromGoal(goalName);
            string[] foundCaps = role.GetCapabilityFromRole(foundRole);

            sb.AppendFormat("Goal: {0}\r\n", goalName);
            sb.AppendFormat("Role to achieve goal: {0}\r\n", foundRole);
            sb.AppendFormat("Capability:\r\n");

            foreach (string s in foundCaps)
            {
                sb.AppendFormat("{0}\r\n", s);
            }

            sb.AppendFormat("\r\n");

            Arrow[] arr = protocol.TrackSequence(foundRole);

            sb.AppendFormat("Sequence:\r\n");

            for (int i = 0; i < arr.Length; i++)
            {
                sb.AppendFormat("{0}: {1}.{2}\r\n", arr[i].index, agent.GetAgentFromRole(arr[i].to), arr[i].name);
            }

            return sb.ToString();
        }

        public string GoalModel
        {
            set
            {
                _goalPath = value;
            }
            get
            {
                return _goalPath;
            }
        }

        public string RoleModel
        {
            set
            {
                _rolePath = value;
            }
            get
            {
                return _rolePath;
            }
        }

        public string AgentModel
        {
            set
            {
                _agentPath = value;
            }
            get
            {
                return _agentPath;
            }
        }

        public string ProtocolModel
        {
            set
            {
                _protocolPath = value;
            }
            get
            {
                return _protocolPath;
            }
        }
    }
}
