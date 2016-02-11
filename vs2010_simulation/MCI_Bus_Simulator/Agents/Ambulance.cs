using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Environment;

namespace MCI_Bus_Simulator.Agents
{
    public class Ambulance : Agent
    {
        PositionInfo _position;
        Patient _currentPatient;

        public Ambulance(PositionInfo p)
        {
            _position = p;
        }

        public void MoveTo(PositionInfo p)
        {
            _position = p;
        }

        public void pickupPatient(Patient pt)
        {
            _currentPatient = pt;
        }

        public void performPrimaryTriage()
        {
            throw new System.NotImplementedException();
        }
    }
}
