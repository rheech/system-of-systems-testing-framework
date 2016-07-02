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

            // Randomly adjust temp, humidity
            if (rnd.Next(2) == 1)
            {
                temperature += (0.01 * rnd.Next(3));
            }
            else
            {
                temperature -= (0.01 * rnd.Next(3));
            }

            if (rnd.Next(2) == 1)
            {
                humidity += (0.01 * rnd.Next(3));
            }
            else
            {
                humidity -= (0.01 * rnd.Next(3));
            }
        }
    }
}
