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

        public EmergencyCallCenter(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DisasterReport":
                    // Send RescueVehicle to the MCI Area
                    SendMessage(typeof(RescueVehicle), "DispatchRescueVehicle");
                    break;
                case "DeclareMCI":
                    // After Level 1 MCI declaration, dispatch other managers for support
                    SendMessage(typeof(EMS_Manager), "DispatchEMS_Manager");
                    SendMessage(typeof(AmbulanceManager), "DispatchAmbulanceManager");
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
            SendMessage(typeof(EmergencyCallCenter), "DisasterReport");
        }
    }
}