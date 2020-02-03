using System;

namespace HrApp
{
    public class EmployeeIsAttendingException : Exception
    {
        public EmployeeIsAttendingException(string message) : base(message)
        {
            
        }
    }
}
