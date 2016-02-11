using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Agents;

namespace MCI_Bus_Simulator.Environment
{
    public class MCI_Environment
    {
        PositionInfo _positionIncident;
        Patient[] _patientsIncident;

        protected static EmergencyCallCenter _emergencyCallCenter;

        public static void InitializeEnvironment()
        {
            _emergencyCallCenter = new EmergencyCallCenter();
            //_ambulance = new Ambulance(new PositionInfo(10));
            //_hospital = new Hospital();
        }

        public MCI_Environment()
        {

        }
    }
}
