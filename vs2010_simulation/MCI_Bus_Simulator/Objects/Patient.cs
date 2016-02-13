using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCI_Bus_Simulator.Objects
{
    public enum PATIENT_STATUS
    {
        Minor,
        Delayed,
        Immidiate,
        Dead
    }

    public class Patient : MCI_Object
    {
        private PATIENT_STATUS _patientStatus;

        public Patient()
        {
            
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
    }
}
