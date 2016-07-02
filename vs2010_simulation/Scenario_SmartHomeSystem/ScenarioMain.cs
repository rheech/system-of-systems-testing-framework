using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoS_Simulator;
using Scenario_SmartHomeSystem.Objects;
using Scenario_SmartHomeSystem.Agents;

namespace Scenario_SmartHomeSystem
{
    public class ScenarioMain : Simulator
    {
        public Room room;

        ControlCenter _controlCenter;
        Cooler _cooler;
        Heater _heater;
        Humidifier _humidifier;
        Hygrometer _hygrometer;
        Thermometer _thermometer;

        protected override void Initialize()
        {
            room = new Room(this);
            room.temperature = 25;
            room.humidity = 40;

            _controlCenter = new ControlCenter(this, 25.0, 40.0);
            _cooler = new Cooler(this);
            _heater = new Heater(this);
            _humidifier = new Humidifier(this);
            _hygrometer = new Hygrometer(this);
            _thermometer = new Thermometer(this);
        }

        protected override void Run()
        {
        }

        public override string GetMonitoringText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Temperature: {0}\r\n", room.temperature);
            sb.AppendFormat("Humidity: {0}\r\n", room.humidity);

            return sb.ToString();
        }
    }
}
