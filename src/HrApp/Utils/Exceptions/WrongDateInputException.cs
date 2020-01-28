using System;

namespace HrApp
{
    public class WrongDateInputException : Exception
    {
        public WrongDateInputException(string message) : base(message)
        {
            
        }
    }
}
