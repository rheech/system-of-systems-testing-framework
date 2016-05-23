using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using SoS_Simulator.Objects;
using SoS_Simulator.Agents;
using Scenario_MCI.Agents;
using Scenario_MCI.Objects;

namespace SoS_Simulator
{
    public class ScenarioMain : Simulator
    {
        //public static TesterAgent.TestMonitor t = new TesterAgent.TestMonitor();
        public static Queue<Patient> patients;
        public static List<Patient> savedPatients;

        // other agents
        Ambulance _ambulance;
        Hospital _hospital;
        EmergencyCallCenter _callCenter;
        Disaster _disaster;
        EMSVehicle _emsVehicle;
        RescueVehicle _rescueVehicle;


        protected override void Initialize()
        {
            _hospital = new Hospital(15);
            _ambulance = new Ambulance(15);
            _callCenter = new EmergencyCallCenter();
            _disaster = new Disaster(5);
            _emsVehicle = new EMSVehicle();
            _rescueVehicle = new RescueVehicle();

            //SetPatients(10);
            savedPatients = new List<Patient>();
        }

        protected override void Run()
        {
            _callCenter.ReportDisaster(_disaster);
        }
    }
}
