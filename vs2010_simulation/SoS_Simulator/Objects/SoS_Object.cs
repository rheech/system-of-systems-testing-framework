using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoS_Simulator.Objects
{
    /// <summary>
    /// An abstract class to define environmental objects in SoS simulator
    /// </summary>
    public abstract class SoS_Object
    {
        private delegate void TickEventHandler();
        private static event TickEventHandler Tick;
        private static int _numCycle = 0;
        private Simulator _simulator;

        /// <summary>
        /// Initializes the new instance of environmental object in SoS simulator.
        /// </summary>
        /// <param name="simulator">A simulator related to the object.</param>
        public SoS_Object(Simulator simulator)
        {
            SoS_Object.Tick += this.OnTickInternal;
            _simulator = simulator;
        }

        /// <summary>
        /// SoS object destructor
        /// </summary>
        ~SoS_Object()
        {
            SoS_Object.Tick -= this.OnTickInternal;
        }

        /// <summary>
        /// Retrive the current simulator
        /// </summary>
        /// <typeparam name="T">The type of the current simulator.</typeparam>
        /// <returns>Current simulator</returns>
        public T Simulation<T>()
        {
            object temp;
            
            // Check if the type is derived from Simulator
            if (typeof(Simulator) == typeof(T))
            {
                temp = _simulator;
                return (T)temp;
            }
            else if(_simulator.GetType() == typeof(T))
            {
                return (T)Convert.ChangeType(_simulator, typeof(T));
            }

            throw new ApplicationException("Simulator casting error.");
        }

        /// <summary>
        /// Reset all event handlers
        /// </summary>
        public static void ResetEventHandler()
        {
            Tick = null;
            _numCycle = 0;
        }

        /// <summary>
        /// Update all objects in the simulator
        /// </summary>
        public static void RaiseTick()
        {
            _numCycle++;

            if (Tick != null)
            {
                Tick();
            }
        }

        /// <summary>
        /// Tick event callback
        /// </summary>
        private void OnTickInternal()
        {
            OnTick();
        }

        /// <summary>
        /// An overridable Tick method for derived objects
        /// </summary>
        protected virtual void OnTick()
        {
        }

        public int Cycle
        {
            get
            {
                return _numCycle;
            }
        }
    }
}