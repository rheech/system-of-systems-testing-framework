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
            XmlRoleModel role = new XmlRoleModel("Resources\\" + txtRole.Text);
            XmlProtocolModel protocol = new XmlProtocolModel("Resources\\" + txtProtocol.Text);

            StringBuilder sb = new StringBuilder();
            string a = role.GetRoleFromGoal("LocatePatient");
            string b = role.GetCapabilityFromRole(a);
            
            sb.AppendFormat("{0}\r\n{1}", a, b);

            Arrow[] arr = protocol.TrackSequence(a);

            MessageBox.Show(arr.ToString());
        }
    }
}
