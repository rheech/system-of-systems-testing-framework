using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Environment;

namespace MCI_Bus_Simulator.Agents
{
    public class Hospital : Agent
    {
        private PositionInfo _position;
        private List<Patient> _patients;

        public Hospital(PositionInfo p)
        {
            p = _position;
            _patients = new List<Patient>();
        }

        public void receivePatient(Patient pt)
        {
            _patients.Add(pt);
        }

        public PositionInfo Position
        {
            get
            {
                return _position;
            }
            set
            {
                value = _position;
            }
        }
    }
}