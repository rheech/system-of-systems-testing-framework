using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI_Single;
using Scenario_MCI_Single.Objects;
using Scenario_MCI_Single.Abstract;

namespace Scenario_MCI_Single.Agents
{
    public class Hospital : MCI_Agent
    {
        private int numOfAvailableBeds;
        private List<Patient> _patients;

        public Hospital(ScenarioMain simulator, int AvailableBeds) : base(simulator)
        {
            numOfAvailableBeds = AvailableBeds;
            _patients = new List<Patient>();
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "CheckBedAvailability":
                    SendMessage(typeof(EMSVehicle), "ProvideBedAvailability", numOfAvailableBeds);
                    break;
                case "PATIENT_ARRIVAL":
                    SendMessage(typeof(EmergencyCallCenter), "CHECK_MORE_PATIENTS");
                    break;
                case "FIND_HOSPITAL":
                    SendMessage(typeof(Ambulance), "HOSPITAL_LOCATION");
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