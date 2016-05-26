using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Agents;
using Scenario_MCI.Objects;

namespace Scenario_MCI.Agents
{
    public class Ambulance : Agent, IPosition
    {
        private enum VEHICLE_STATUS
        {
            PARKED,
            TO_DISASTER_AREA,
            TO_HOSPITAL
        }

        VEHICLE_STATUS _vehicleStatus;
        Patient _currentPatient;
        int _currentPosition, _destination;

        public Ambulance(int x)
        {
            _currentPosition = x;
            _vehicleStatus = VEHICLE_STATUS.PARKED;
        }

        public void PickupPatient(Patient pt)
        {
            _currentPatient = pt;
        }

        public Patient ReleasePatient(Patient pt)
        {
            Patient rtnPatient;
            rtnPatient = _currentPatient;

            _currentPatient = null;

            return rtnPatient;
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "RequestAmbulance":
                    SendMessage(typeof(EMSVehicle), "TransportComplete");
                    break;
                default:
                    break;
            }
        }

        public void MoveTo(int x)
        {
            _destination = x;
            _vehicleStatus = VEHICLE_STATUS.TO_DISASTER_AREA;
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        public int X
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
            }
        }
    }
}
