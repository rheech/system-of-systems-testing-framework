using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskModelReader
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TaskModel model = new TaskModel("Resources\\taskmodel.xml");
            TaskSequenceSet seq;

            //model.RetrieveGoalList();
            seq = model.RetrieveTaskSequence("Treatment");

            //MessageBox.Show(model.test());
        }
    }
}
