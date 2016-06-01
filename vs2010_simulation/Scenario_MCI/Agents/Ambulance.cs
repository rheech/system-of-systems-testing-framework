﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_MCI.Abstract;
using Scenario_MCI.Objects;

namespace Scenario_MCI.Agents
{
    public class Ambulance : MCI_Agent
    {
        enum AMBULANCE_POSITION
        {
            StagingArea,
            Field,
            Hospital
        };

        AMBULANCE_POSITION position;

        public Ambulance(ScenarioMain simulator)
            : base(simulator)
        {
        }

        protected override void OnMessageReceived(object from, Type target, string msgType, params object[] info)
        {
            switch (msgType)
            {
                case "RequestStandBy":
                    position = AMBULANCE_POSITION.StagingArea;
                    break;
                case "DispatchAmbulance": // Ambulance dispatch request from staging area to MCI field
                    position = AMBULANCE_POSITION.Field;
                    // Go to MCI field
                    SendMessage(typeof(EMS_Manager), "FieldArrivalReport");
                    break;
                case "RequestPatientTransfer":
                    // Transport patient
                    Patient p = (Patient)info[0];
                    SendMessage(typeof(Hospital), "DispatchPatient", p);
                    SendMessage(typeof(EMS_Manager), "TransferComplete");
                    position = AMBULANCE_POSITION.StagingArea;
                    break;
                default:
                    break;
            }
        }
    }
}
