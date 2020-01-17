using System;
namespace HrApp
{
    public class BookingPropertyIsInvalidException : Exception
    {
        public BookingPropertyIsInvalidException(string message) : base(message)
        {

        }
    }
}
