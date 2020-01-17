using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class BookingIsNotAvailableException : Exception
    {
        public BookingIsNotAvailableException(string message) : base(message)
        {
            
        }
    }
}
