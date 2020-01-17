using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class WrongDateInputException : Exception
    {
        public WrongDateInputException(string message) : base(message)
        {
            
        }
    }
}
