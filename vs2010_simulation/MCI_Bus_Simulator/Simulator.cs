﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCI_Bus_Simulator.Agents;
using MCI_Bus_Simulator.Objects;

namespace MCI_Bus_Simulator
{
    public class Simulator
    {
        //public static TesterAgent.TestMonitor t = new TesterAgent.TestMonitor();
        public static Queue<Patient> patients;
        public static List<Patient> savedPatients;

        public MonitorAgent MonitorAgent;
        Ambulance _ambulance;
        Hospital _hospital;
        EmergencyCallCenter _callCenter;
        Disaster _disaster;

        public Simulator()
        {
            InitializeSimulation();
        }

        public void InitializeSimulation()
        {
            Agent.ResetEventHandler();
            MCI_Object.ResetEventHandler();

            MonitorAgent = new MonitorAgent();
            _hospital = new Hospital(15);
            _ambulance = new Ambulance(15);
            _callCenter = new EmergencyCallCenter();
            _disaster = new Disaster(5);
            SetPatients(10);
            savedPatients = new List<Patient>();
        }

        public void RunSimulator()
        {
            _callCenter.ReportDisaster(_disaster);
            //Agent.SendMessage(typeof(Ambulance), MESSAGE_TYPE.TEST);
        }

        public void SetPatients(int numOfPatients)
        {
            patients = new Queue<Patient>();

            for (int i = 0; i < numOfPatients; i++)
            {
                patients.Enqueue(new Patient());
            }
        }

        public void Tick()
        {
            MCI_Object.RaiseTick();
        }

        public string GetNumbers()
        {
            return String.Format("Waiting patients: {0}\r\nSaved patients: {1}", patients.Count, savedPatients.Count);
        }

        public string UpdateVisualComponent()
        {
            StringBuilder sb = new StringBuilder();
            string sDisaster, sHospital;
            string sCombined;
            
            sDisaster = String.Format("{0}{1}", new String(' ', _disaster.X), "X");
            sHospital = String.Format("{0}{1}", new String(' ', _hospital.X), "H");

            sCombined = MergeTwoString(sDisaster, sHospital);

            sb.AppendLine(sCombined);
            sb.AppendFormat("{0}{1}", new String(' ', _ambulance.X), "A");

            return sb.ToString();
        }

        private string MergeTwoString(string exp1, string exp2)
        {
            char[] output;

            if (exp1.Length > exp2.Length)
            {
                exp2 = exp2.PadRight(exp1.Length);
            }
            else if (exp1.Length < exp2.Length)
            {
                exp1 = exp1.PadRight(exp2.Length);
            }

            output = new char[exp1.Length];

            for (int i = 0; i < exp1.Length; i++)
            {
                if (exp1[i] != ' ' && exp2[i] == ' ')
                {
                    output[i] = exp1[i];
                }
                else if (exp1[i] == ' ' && exp2[i] != ' ')
                {
                    output[i] = exp2[i];
                }
                else if (exp1[i] == ' ' && exp2[i] == ' ')
                {
                    output[i] = ' ';
                }
                else
                {
                    output[i] = 'Z';
                }
            }

            return new String(output);
        }
    }
}