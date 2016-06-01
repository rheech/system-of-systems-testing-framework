using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using SoS_Simulator.Agents;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Agents
{
    public class RescueVehicle : MCI_Agent
    {
        bool isDispatched;

        public RescueVehicle(ScenarioMain simulator)
            : base(simulator)
        {
            isDispatched = false;
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DispatchRescueVehicle":
                    // Declare Level 1 MCI after the investigation
                    isDispatched = true;
                    SendMessage(typeof(EmergencyCallCenter), "DeclareMCI");
                    this.StartTriage();
                    break;
                case "TreatmentComplete": // If treatment is complete
                    SendMessage(typeof(EmergencyCallCenter), "MCIComplete");
                    break;
                case "TransportComplete":
                    SendMessage(typeof(EmergencyCallCenter), "EquipmentReleaseComplete"); // terminal state
                    //Simulation.Finish();
                    break;
                default:
                    break;
            }
        }

        // Perform medical S.T.A.R.T. Triage
        private void StartTriage()
        {
            // We assume that StartTriage is already done (not implemented)

            // After the triage is complete, report to emergency call center
            SendMessage(typeof(EmergencyCallCenter), "TriageComplete");
            
            // Request EMS_Manager for patient treatment
            SendMessage(typeof(EMS_Manager), "RequestPatientTreatment");
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
