using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI;
using Scenario_MCI.Objects;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Agents
{
    public class Hospital : MCI_Agent
    {
        private int bedAvailability;
        private List<Patient> _patients;

        public Hospital(ScenarioMain simulator, int AvailableBeds)
            : base(simulator)
        {
            bedAvailability = AvailableBeds;
            _patients = new List<Patient>();
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "RequestBedAvailability":
                    SendMessage(typeof(EMS_Manager), "ProvideBedAvailability", bedAvailability);
                    break;
                case "DispatchPatient":
                    Patient p = (Patient)info[0];
                    _patients.Add(p);
                    bedAvailability--;
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