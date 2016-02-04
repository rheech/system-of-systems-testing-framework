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

            string a = role.GetRoleFromGoal("LocatePatient");

            MessageBox.Show(a);

            MessageBox.Show(role.GetCapabilityFromRole(a));
        }
    }
}
