using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_MCI_Single.Abstract;

namespace Scenario_MCI_Single.Objects
{
    public class Disaster : MCI_Object, IPosition
    {
        int _x;

        public Disaster(ScenarioMain simulator, int x) : base(simulator)
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
