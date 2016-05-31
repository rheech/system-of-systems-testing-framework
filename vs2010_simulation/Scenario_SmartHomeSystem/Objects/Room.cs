using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_SmartHomeSystem.Abstract;

namespace Scenario_SmartHomeSystem.Objects
{
    public class Room : SmartHome_Object
    {
        Random rnd;

        public Room(ScenarioMain simulator)
            : base(simulator)
        {
            rnd = new Random();
        }

        public double temperature;
        public double humidity;

        protected override void OnTick()
        {
            base.OnTick();

            /*if (rnd.Next(1) == 1)
            {
                temperature += (0.01 * rnd.Next(10));
            }
            else
            {
                temperature -= (0.01 * rnd.Next(10));
            }

            if (rnd.Next(1) == 1)
            {
                humidity += (0.01 * rnd.Next(10));
            }
            else
            {
                humidity -= (0.01 * rnd.Next(10));
            }*/
        }
    }
}
