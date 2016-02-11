using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Environment;

namespace MCI_Bus_Simulator.Agents
{
    public class EmergencyCallCenter : Agent
    {
        private PositionInfo _disasterPosition;
        private int _reportedNumOfPatients;
        private int _savedPatients;

        private Hospital _hospital;
        private Ambulance _ambulance;

        public EmergencyCallCenter()
        {
            _hospital = new Hospital(new PositionInfo(3));
        }

        // Set accident info
        public void activateMCI(int numOfPatients, PositionInfo incidentPosition)
        {
            _disasterPosition = incidentPosition;
            _reportedNumOfPatients = numOfPatients;
        }

        public void assignAmbulance()
        {
            _ambulance = new Ambulance(_hospital.Position);
        }

        public void reportMCI()
        {
            Console.WriteLine("MCI broadcasting: ...");
        }

        public void reportSavedPatient()
        {
            Console.WriteLine("Saved patients: {0}", _savedPatients);
        }
    }
}