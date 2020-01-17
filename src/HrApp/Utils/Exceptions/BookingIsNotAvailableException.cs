using System;

namespace HrApp
{
    public class BookingIsNotAvailableException : Exception
    {
        public BookingIsNotAvailableException(string message) : base(message)
        {
            
        }
    }
}
