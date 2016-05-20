using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;

namespace SoS_Simulator.Agents
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

        protected override void OnMessageReceived(object from, Type target, MESSAGE_TYPE msgType, params object[] info)
        {
            switch (msgType)
            {
                case MESSAGE_TYPE.RequestAmbulance:
                    SendMessage(typeof(EMSVehicle), MESSAGE_TYPE.TransportComplete);
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
            int diff;
            int unit;

            base.OnTick();

            if (_currentPosition != _destination)
            {
                diff = _destination - _currentPosition;
                unit = diff / Math.Abs(diff);

                _currentPosition += unit;

                // arrived to destination
                if (_currentPosition == _destination)
                {
                    // to disaster area
                    if (_vehicleStatus == VEHICLE_STATUS.TO_DISASTER_AREA)
                    {
                        _currentPatient = Simulator.patients.Dequeue();
                        SendMessage(typeof(Ambulance), MESSAGE_TYPE.PATIENT_PICKEDUP);
                    }
                    else if (_vehicleStatus == VEHICLE_STATUS.TO_HOSPITAL) // to hospital
                    {
                        Simulator.savedPatients.Add(_currentPatient);
                        _currentPatient = null;
                        _vehicleStatus = VEHICLE_STATUS.PARKED;

                        SendMessage(typeof(Hospital), MESSAGE_TYPE.PATIENT_ARRIVAL);
                    }
                }

                /*if (_currentPosition == _destination && MCI_Environment.patients.Count > 0)
                {
                    _currentPatient = MCI_Environment.patients.Dequeue();
                }*/
            }
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
