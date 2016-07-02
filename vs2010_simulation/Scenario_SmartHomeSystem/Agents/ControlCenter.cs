using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario_SmartHomeSystem.Abstract;

namespace Scenario_SmartHomeSystem.Agents
{
    public class ControlCenter : SmartHome_Agent
    {
        double _goalTemp, _goalHumid;

        public ControlCenter(ScenarioMain simulator, double goalTemperature, double goalHumidity)
            : base(simulator)
        {
            _goalTemp = goalTemperature;
            _goalHumid = goalHumidity;
        }

        protected override void OnMessageReceived(object from, Type target, string msgText, params object[] info)
        {
            switch (msgText)
            {
                case "CurrentTemperature":
                    ActTemperature((double)info[0]);
                    break;
                case "CurrentHumidity":
                    ActHumidity((double)info[0]);
                    break;
                default:
                    break;
            }
        }

        private void CheckTemperature()
        {
            SendMessage(typeof(Thermometer), "CheckTemp");
        }

        private void CheckHumidity()
        {
            SendMessage(typeof(Hygrometer), "CheckHumid");
        }

        private void Cool()
        {
            SendMessage(typeof(Cooler), "Cool");
        }

        private void Heat()
        {
            SendMessage(typeof(Heater), "Heat");
        }

        private void Dehumidify()
        {
            SendMessage(typeof(Dehumidifier), "DrawInMoist");
        }

        private void Humidify()
        {
            SendMessage(typeof(Humidifier), "ReleaseMoist");
        }

        private void ActTemperature(double currentTemperature)
        {
            double diff;
            diff = currentTemperature - _goalTemp;

            if (diff < 0)
            {
                Heat();
            }
            else if (diff > 0)
            {
                Cool();
            }
        }

        private void ActHumidity(double currentHumidity)
        {
            double diff;
            diff = currentHumidity - _goalHumid;

            if (diff < 0)
            {
                Humidify();
            }
            else if (diff > 0)
            {
                Dehumidify();
            }
        }

        protected override void OnTick()
        {
            base.OnTick();

            //CheckTemperature();
            //CheckHumidity();
            ActTemperature(Simulation.room.temperature);
            ActHumidity(Simulation.room.humidity);
        }
    }
}
