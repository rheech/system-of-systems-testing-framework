using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_MCI.Abstract;

namespace Scenario_MCI.Objects
{
    public enum PATIENT_STATUS
    {
        Minor,
        Delayed,
        Immediate,
        Dead
    }

    public class Patient : MCI_Object
    {
        private int _delayed;
        private PATIENT_STATUS _patientStatus;

        public Patient(ScenarioMain simulator) : base(simulator)
        {
            _delayed = 0;
        }

        public PATIENT_STATUS Status
        {
            get
            {
                return _patientStatus;
            }
            set
            {
                _patientStatus = value;
            }
        }

        protected override void OnTick()
        {
            _delayed++;
        }
    }
}
