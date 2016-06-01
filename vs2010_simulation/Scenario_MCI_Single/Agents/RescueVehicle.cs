using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using SoS_Simulator.Agents;
using Scenario_MCI_Single.Abstract;

namespace Scenario_MCI_Single.Agents
{
    public class RescueVehicle : MCI_Agent
    {
        public RescueVehicle(ScenarioMain simulator) : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "DispatchCommand":
                    SendMessage(typeof(EmergencyCallCenter), "DeclareMCI");
                    
                    SendMessage(typeof(RescueVehicle), "AssignTriagePosition");
                    SendMessage(typeof(EMSVehicle), "AssignTreatmentPosition");
                    SendMessage(typeof(EMSVehicle), "AssignTransportationPosition");
                    break;
                case "AssignTriagePosition":
                    SendMessage(typeof(RescueVehicle), "ProvidePatientCount");
                    break;
                default:
                    break;
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
