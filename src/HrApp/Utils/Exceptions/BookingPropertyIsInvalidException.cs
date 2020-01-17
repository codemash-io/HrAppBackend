using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class BookingPropertyIsInvalidException : Exception
    {
        public BookingPropertyIsInvalidException(string message) : base(message)
        {

        }
    }
}
