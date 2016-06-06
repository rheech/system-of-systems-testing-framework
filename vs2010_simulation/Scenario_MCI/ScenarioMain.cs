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
        public List<Patient> patients;
        public int immediate, delayed, minor, dead;

        // other agents
        AmbulanceManager _ambulanceManager;
        Ambulance _ambulance;
        Hospital _hospital;
        EmergencyCallCenter _callCenter;
        EMSVehicle _emsVehicle;
        RescueVehicle _rescueVehicle;

        // environment
        public Disaster disaster;


        protected override void Initialize()
        {
            _hospital = new Hospital(this, 15);
            _ambulanceManager = new AmbulanceManager(this);
            _ambulance = new Ambulance(this);
            _callCenter = new EmergencyCallCenter(this);
            disaster = new Disaster(this);
            _emsVehicle = new EMSVehicle(this);
            _rescueVehicle = new RescueVehicle(this);

            //SetPatients(10);
            patients = new List<Patient>();

            immediate = 3;
            delayed = 4;
            minor = 5;
            dead = 2;

            for (int i = 0; i < immediate; i++)
            {
                patients.Add(new Patient(this, PATIENT_STATUS.Immediate));
            }

            for (int i = 0; i < delayed; i++)
            {
                patients.Add(new Patient(this, PATIENT_STATUS.Delayed));
            }

            for (int i = 0; i < minor; i++)
            {
                patients.Add(new Patient(this, PATIENT_STATUS.Minor));
            }

            for (int i = 0; i < dead; i++)
            {
                patients.Add(new Patient(this, PATIENT_STATUS.Dead));
            }
        }

        public int Total
        {
            get
            {
                return minor + delayed + immediate + dead;
            }
        }

        public int transferred
        {
            get
            {
                return Total - patients.Count;
            }
        }

        protected override void Run()
        {
            _callCenter.ReportDisaster(disaster);
        }

        public override string GetMonitoringText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Patients: {0}\r\n\r\n", Total);
            sb.AppendFormat("Minor: {0}\r\n", minor);
            sb.AppendFormat("Immediate: {0}\r\n", immediate);
            sb.AppendFormat("Delayed: {0}\r\n", delayed);
            sb.AppendFormat("Dead: {0}\r\n", dead);
            sb.Append("\r\n");
            sb.AppendFormat("Transferred: {0}\r\n", transferred);

            return sb.ToString();
        }
    }
}
