using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Objects
{
    public abstract class SoS_Object
    {
        private delegate void TickEventHandler();
        private static event TickEventHandler Tick;
        private Simulator _simulator;

        public SoS_Object(Simulator simulator)
        {
            SoS_Object.Tick += this.OnTickInternal;
            _simulator = simulator;
        }

        ~SoS_Object()
        {
            SoS_Object.Tick -= this.OnTickInternal;
        }

        public T Simulation<T>()
        {
            if (_simulator.GetType() == typeof(T))
            {
                return (T)Convert.ChangeType(_simulator, typeof(T));
            }

            throw new ApplicationException("Simulator conversion error.");
        }

        public static void ResetEventHandler()
        {
            Tick = null;
        }

        public static void RaiseTick()
        {
            if (Tick != null)
            {
                Tick();
            }
        }

        private void OnTickInternal()
        {
            OnTick();
        }

        protected virtual void OnTick()
        {
        }
    }
}
