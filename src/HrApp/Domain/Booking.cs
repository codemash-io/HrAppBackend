using System;
using System.Collections.Generic;

namespace HrApp
{
    public class Booking
    {
        public string MeetingRoomName { get; set; }
        public string Organizer { get; set; }
        public DateTime StartTime { get; set; } // 2019, input month, input day, input hours, input minutes (only every 15 minutes option is available)
        public DateTime EndTime { get; set; }
        public List<string> Participants { get; set; }
        public string Subject { get; set; }

        
        public Booking(string meetingRoomName, string authorEmployee, DateTime startTime, DateTime endTime, List<string> employees, string description)
        {
            MeetingRoomName = meetingRoomName;
            Organizer = authorEmployee;
            StartTime = startTime;
            EndTime = endTime;
            Participants = employees;
            Subject = description;

            BookingPropertiesCheck();
        }

        private void BookingPropertiesCheck()
        {
            if (String.IsNullOrEmpty(MeetingRoomName))
                throw new BookingPropertyIsInvalidException("MeetingRoomName property is null");

            if (!Enum.IsDefined(typeof(MeetingRooms), MeetingRoomName))
                throw new BookingPropertyIsInvalidException("MeetingRoomName is invalid");

            //Organizer.CheckEmployeeProperties();

            if(StartTime == DateTime.MinValue)
                throw new BookingPropertyIsInvalidException("StartTime property is null");

            if(StartTime.Minute % 15 != 0 || EndTime.Minute % 15 != 0)
                throw new BookingPropertyIsInvalidException("Start and end time minutes should be 15 iterative (ex.: 15:15 or 21:45)");

            if (EndTime == DateTime.MinValue)
                throw new BookingPropertyIsInvalidException("EndTime property is null");

            if (StartTime.CompareTo(EndTime) > 0)
                throw new BookingPropertyIsInvalidException("StartTime must be earlier than EndTime");

            if (Participants.Count < 1)
                throw new BookingPropertyIsInvalidException("Meeting room must be attended by at least 2 employees");

           /* foreach (var employee in Participants)
            {
                employee.CheckEmployeeProperties();
            }*/
        }
    }
}
