using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker
{
    public class ApplicationError : ApplicationException
    {
        public ApplicationError()
            : base()
        {
        }
        
        public ApplicationError(string message)
            : base(message)
        {
        }
    }
}
