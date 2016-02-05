using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestCaseGenerator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateTestCase("LocatePatient");
        }

        private void GenerateTestCase(string goalName)
        {
            XmlRoleModel role = new XmlRoleModel("Resources\\" + txtRole.Text);
            XmlProtocolModel protocol = new XmlProtocolModel("Resources\\" + txtProtocol.Text);
            XmlAgentModel agent = new XmlAgentModel("Resources\\" + txtAgent.Text);
            
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

            txtOutput.Text = sb.ToString();
        }

        private void lstGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTestCase(lstGoals.SelectedItem.ToString());
        }
    }
}
