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
        private Patient[] _patients;

        public Hospital(PositionInfo p)
        {
            p = _position;
        }

        public void receivePatient()
        {
            throw new System.NotImplementedException();
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
