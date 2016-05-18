using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCaseGenerator.Xml;

namespace TestCaseGenerator
{
    public struct Arrow
    {
        public int index;
        public string agent_to;
        public string message;
        public string role_to;

        public override string ToString()
        {
            return String.Format("{0}.{1}", agent_to, message);
        }
    }

    public struct TestInfo
    {
        public string goalName;
        public string foundRole;
        public string[] foundCaps;
        public Arrow[] sequence;

        public override string ToString()
        {
            string output;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Goal: {0}\r\n", goalName);
            sb.AppendFormat("Role to achieve goal: {0}\r\n", foundRole);
            sb.AppendFormat("Capability:\r\n");

            foreach (string s in foundCaps)
            {
                sb.AppendFormat("{0}\r\n", s);
            }

            sb.AppendFormat("\r\n");

            sb.AppendFormat("Sequence:\r\n");

            for (int i = 0; i < sequence.Length; i++)
            {
                sb.AppendFormat("{0}: {1}.{2}\r\n", sequence[i].index, sequence[i].agent_to, sequence[i].message);
            }

            try
            {
                output = sb.ToString();
            }
            catch (Exception)
            {
                output = base.ToString();
            }

            return output;
        }
    }

    public class TCGenerator
    {
        private string _rolePath;
        private string _agentPath;
        private string _taskPath;

        public TCGenerator()
        {

        }

        public TestInfo GenerateTestCase(string goalName)
        {
            RoleModel role = new RoleModel(RoleModel);
            AgentModel agent = new AgentModel(AgentModel);
            TaskModel task = new TaskModel(TaskModel);

            TestInfo info = new TestInfo();
            info.goalName = goalName;
            /*info.foundRole = role.GetRoleFromGoal(goalName);
            info.foundCaps = role.GetCapabilityFromRole(info.foundRole);
            info.sequence = protocol.TrackSequence(info.foundRole, agent);*/

            TaskSequenceSet tSet;
            tSet = task.RetrieveTaskSequence(goalName);

            agent.GetAgentFromRole("BaseAmbulance");

            role.GetTestSequenceFromTask(tSet);

            

            
            return info;

            /*StringBuilder sb = new StringBuilder();

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

            return sb.ToString();*/
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

        public string TaskModel
        {
            set
            {
                _taskPath = value;
            }
            get
            {
                return _taskPath;
            }
        }
    }
}
