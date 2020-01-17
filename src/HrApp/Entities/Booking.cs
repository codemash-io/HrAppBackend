using System;
using System.Collections.Generic;
using System.Text;
using Meeting_Room_Booking_System.Exceptions;
using Meeting_Room_Booking_System.Utils;

namespace Meeting_Room_Booking_System
{
    public class Booking
    {
        public string MeetingRoomName { get; set; }
        public Employee AuthorEmployee { get; set; }
        public DateTime StartTime { get; set; } // 2019, input month, input day, input hours, input minutes (only every 15 minutes option is available)
        public DateTime EndTime { get; set; }
        public Employee[] Employees { get; set; }
        public string Description { get; set; }

        public Booking()
        {
            
        }

        public Booking(string meetingRoomName, Employee authorEmployee, DateTime startTime, DateTime endTime, Employee[] employees, string description)
        {
            MeetingRoomName = meetingRoomName;
            AuthorEmployee = authorEmployee;
            StartTime = startTime;
            EndTime = endTime;
            Employees = employees;
            Description = description;

            BookingPropertiesCheck();
        }

        private void BookingPropertiesCheck()
        {
            if (String.IsNullOrEmpty(MeetingRoomName))
                throw new BookingPropertyIsInvalidException("MeetingRoomName property is null");

            if (!Enum.IsDefined(typeof(MeetingRooms), MeetingRoomName))
                throw new BookingPropertyIsInvalidException("MeetingRoomName is invalid");

            AuthorEmployee.CheckEmployeeProperties();

            if(StartTime == DateTime.MinValue)
                throw new BookingPropertyIsInvalidException("StartTime property is null");

            if(StartTime.Minute % 15 != 0 || EndTime.Minute % 15 != 0)
                throw new BookingPropertyIsInvalidException("Start and end time minutes should be 15 iterative (ex.: 15:15 or 21:45)");

            if (EndTime == DateTime.MinValue)
                throw new BookingPropertyIsInvalidException("EndTime property is null");

            if (StartTime.CompareTo(EndTime) > 0)
                throw new BookingPropertyIsInvalidException("StartTime must be earlier than EndTime");

            if (Employees.Length < 1)
                throw new BookingPropertyIsInvalidException("Meeting room must be attended by at least 2 employees");

            foreach (Employee employee in Employees)
            {
                employee.CheckEmployeeProperties();
            }
        }
    }
}
