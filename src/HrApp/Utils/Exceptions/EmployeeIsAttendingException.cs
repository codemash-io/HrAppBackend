﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Meeting_Room_Booking_System.Exceptions
{
    public class EmployeeIsAttendingException : Exception
    {
        public EmployeeIsAttendingException(string message) : base(message)
        {
            
        }
    }
}
