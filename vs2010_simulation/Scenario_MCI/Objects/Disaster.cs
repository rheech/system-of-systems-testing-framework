using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Objects
{
    public class Disaster : SoS_Object, IPosition
    {
        int _x;

        public Disaster(int x)
        {
            _x = x;
        }

        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
    }
}
