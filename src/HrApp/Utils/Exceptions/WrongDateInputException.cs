using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class WrongDateInputException : Exception
    {
        public WrongDateInputException(string message) : base(message)
        {
            
        }
    }
}
