using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator.Objects;

namespace Scenario_MCI.Objects
{
    public enum PATIENT_STATUS
    {
        Minor,
        Delayed,
        Immediate,
        Dead
    }

    public class Patient : SoS_Object
    {
        private int _delayed;
        private PATIENT_STATUS _patientStatus;

        public Patient()
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
