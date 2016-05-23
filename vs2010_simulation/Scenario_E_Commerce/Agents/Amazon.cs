using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;

namespace Scenario_E_Commerce.Agents
{
    public class Amazon : Agent
    {
        private int _reportedNumOfPatients;
        private int _savedPatients;

        public Amazon()
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "":
                    SimulationComplete(false);
                    break;
                default:
                    break;
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        // Received disaster report (beginning of MCI)
        public void ReportDisaster(Disaster disaster)
        {
            _disaster = disaster;
            SendMessage(typeof(EmergencyCallCenter), "ReportDisaster", _disaster.X);
        }
    }
}
