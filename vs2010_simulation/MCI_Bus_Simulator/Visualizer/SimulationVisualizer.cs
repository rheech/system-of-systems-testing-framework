using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MCI_Bus_Simulator.Objects;

namespace MCI_Bus_Simulator.Visualizer
{
    public partial class SimulationVisualizer : Label
    {
        private const string _defText = @"|----|----|----|----|";

        public SimulationVisualizer()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void UpdateGraphics(string expression)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\n", _defText);
            sb.Append(expression);

            this.Text = sb.ToString();
        }

        private void InitializeControl()
        {
            // Courier New, Bold, 18pt
            this.Font = new Font("Courier New", 18, FontStyle.Bold);

            base.Text = _defText;
        }

        public void StartVisualization()
        {
            base.Text = _defText;
        }
    }
}
