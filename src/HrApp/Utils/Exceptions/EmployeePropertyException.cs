using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class EmployeePropertyException : Exception
    {
        public EmployeePropertyException(string message) : base(message)
        {
            
        }
    }
}
