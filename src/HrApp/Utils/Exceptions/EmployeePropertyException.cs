using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class EmployeePropertyException : Exception
    {
        public EmployeePropertyException(string message) : base(message)
        {
            
        }
    }
}
