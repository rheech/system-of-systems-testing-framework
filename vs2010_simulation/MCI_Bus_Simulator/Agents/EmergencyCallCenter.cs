using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Objects;

namespace MCI_Bus_Simulator.Agents
{
    public class EmergencyCallCenter : Agent
    {
        private int _reportedNumOfPatients;
        private int _savedPatients;
        private Disaster _disaster;

        public EmergencyCallCenter()
        {
        }

        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.ReportDisaster:
                    SendMessage(typeof(RescueVehicle), MESSAGE_TYPE.Command);
                    break;
                case MESSAGE_TYPE.RESCUE_COMPLETE:
                    SimulationComplete(false);
                    break;
                default:
                    break;
            }
        }

        // Received disaster report (beginning of MCI)
        public void ReportDisaster(Disaster disaster)
        {
            _disaster = disaster;
            SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ReportDisaster, _disaster.X);
        }
    }
}