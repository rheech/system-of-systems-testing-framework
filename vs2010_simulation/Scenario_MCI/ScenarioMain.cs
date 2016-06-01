using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using SoS_Simulator.Objects;
using SoS_Simulator.Agents;
using Scenario_MCI.Agents;
using Scenario_MCI.Objects;

namespace Scenario_MCI
{
    public class ScenarioMain : Simulator
    {
        //public static TesterAgent.TestMonitor t = new TesterAgent.TestMonitor();
        public static Queue<Patient> patients;
        public static List<Patient> savedPatients;

        // other agents
        AmbulanceManager _ambulance;
        Hospital _hospital;
        EmergencyCallCenter _callCenter;
        EMS_Manager _emsVehicle;
        RescueVehicle _rescueVehicle;

        // environment
        public Disaster disaster;


        protected override void Initialize()
        {
            _hospital = new Hospital(this, 15);
            _ambulance = new AmbulanceManager(this);
            _callCenter = new EmergencyCallCenter(this);
            disaster = new Disaster(this);
            _emsVehicle = new EMS_Manager(this);
            _rescueVehicle = new RescueVehicle(this);

            //SetPatients(10);
            savedPatients = new List<Patient>();
        }

        protected override void Run()
        {
            _callCenter.ReportDisaster(disaster);
        }

        public override string GetMonitoringText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Patients: \r\n");
            sb.AppendFormat("Minor: \r\n");
            sb.AppendFormat("Immediate: \r\n");
            sb.AppendFormat("Delayed: \r\n");
            sb.Append("\r\n");
            sb.AppendFormat("Saved: \r\n");
            sb.AppendFormat("Dead: \r\n");

            return sb.ToString();
        }
    }
}
