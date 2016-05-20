using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;

namespace SoS_Simulator.Agents
{
    public class Hospital : Agent, IPosition
    {
        private List<Patient> _patients;
        private int _x;

        public Hospital(int x)
        {
            _x = x;
            _patients = new List<Patient>();
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "CheckBedAvailability":
                    SendMessage(typeof(EMSVehicle), "ProvideBedAvailability");
                    break;
                case "PATIENT_ARRIVAL":
                    SendMessage(typeof(EmergencyCallCenter), "CHECK_MORE_PATIENTS");
                    break;
                case "FIND_HOSPITAL":
                    SendMessage(typeof(Ambulance), "HOSPITAL_LOCATION", _x);
                    break;
                default:
                    break;
            }
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
    }
}