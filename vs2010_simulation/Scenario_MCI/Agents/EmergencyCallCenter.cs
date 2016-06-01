using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI.Objects;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Agents
{
    public class EmergencyCallCenter : MCI_Agent
    {
        public int numPatients;

        public EmergencyCallCenter(ScenarioMain simulator) : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DisasterReport":
                    //SendMessage(typeof(EmergencyCallCenter), MESSAGE_TYPE.ReportDisaster);
                    SendMessage(typeof(RescueVehicle), "DispatchCommand");
                    break;
                case "DeclareMCI":
                    SendMessage(typeof(EMS_Manager), "DispatchCommand");
                    SendMessage(typeof(AmbulanceManager), "DispatchCommand");
                    break;
                case "ReportPatientCount":
                    numPatients = (int)info[0];
                    break;
                case "RESCUE_COMPLETE":
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
            SendMessage(typeof(EmergencyCallCenter), "DisasterReport");
        }
    }
}