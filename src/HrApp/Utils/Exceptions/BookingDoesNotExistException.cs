using System;

namespace HrApp
{
    public class BookingDoesNotExistException : Exception
    {
        public BookingDoesNotExistException(string message) : base(message)
        {
            
        }
    }
}
