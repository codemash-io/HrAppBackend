using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class BookingIsNotAvailableException : Exception
    {
        public BookingIsNotAvailableException(string message) : base(message)
        {
            
        }
    }
}
