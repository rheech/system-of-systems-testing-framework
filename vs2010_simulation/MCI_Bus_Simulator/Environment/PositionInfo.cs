using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCI_Bus_Simulator.Environment
{
    public class PositionInfo
    {
        int _x;

        public PositionInfo(int x)
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
                if (value < 0)
                {
                    throw new Exception("Invalid position");
                }

                _x = value;
            }
        }
    }
}
