using System;
using System.Collections.Generic;
using System.Text;

namespace GraphTutorial.Exceptions
{
    public class EventIsNonExistentException : Exception
    {
        public EventIsNonExistentException(string message) : base(message)
        {

        }
    }
}
