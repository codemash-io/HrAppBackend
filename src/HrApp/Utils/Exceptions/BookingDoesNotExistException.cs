using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class BookingDoesNotExistException : Exception
    {
        public BookingDoesNotExistException(string message) : base(message)
        {
            
        }
    }
}
