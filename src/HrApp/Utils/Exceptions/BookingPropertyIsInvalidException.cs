using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class BookingPropertyIsInvalidException : Exception
    {
        public BookingPropertyIsInvalidException(string message) : base(message)
        {

        }
    }
}
