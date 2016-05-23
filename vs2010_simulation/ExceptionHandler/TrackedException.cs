using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionHandler
{
    public class TrackedException : ApplicationException
    {
        public TrackedException()
            : base()
        {
        }

        public TrackedException(string message)
            : base(message)
        {
        }
    }
}
