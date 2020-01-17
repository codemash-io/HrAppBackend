using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class BookingDoesNotExistException : Exception
    {
        public BookingDoesNotExistException(string message) : base(message)
        {
            
        }
    }
}
