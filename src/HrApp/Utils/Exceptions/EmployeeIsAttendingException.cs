using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp
{
    public class EmployeeIsAttendingException : Exception
    {
        public EmployeeIsAttendingException(string message) : base(message)
        {
            
        }
    }
}
