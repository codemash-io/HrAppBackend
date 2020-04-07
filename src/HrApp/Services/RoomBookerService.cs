using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrApp
{
    public class RoomBookerService : IRoomBooker
    {
        public IGraphRepository GraphRepository { get; set; }

        public async Task<bool> BookRoom(Booking booking)
        {
            //checks if selected time available to book
            var dateEvents = await GraphRepository.GetCalendarEventsByDate(
                booking.MeetingRoomName.ToString(), booking.StartTime, booking.EndTime);

            if (dateEvents.Count > 1 && dateEvents != null)
                throw new BusinessException("Selected room at this time is ocupied");
            
            if(dateEvents.Count == 1)
            {
                var date = DateTime.Parse(dateEvents[0].End.DateTime);
                if(date > booking.StartTime.ToUniversalTime())
                    throw new BusinessException("Selected room at this time is ocupied");
            }
            //----------------------------------------------------------

            var roomDetails = await GraphRepository.GetSelectedRoomCalendarDetails(booking.MeetingRoomName);
            var calendarEvent = await CreateEventFromBooking(booking, roomDetails);
            var eventdId = await GraphRepository.CreateEvent(calendarEvent);

            if (string.IsNullOrEmpty(eventdId))
                throw new BusinessException("Something went wrong. Event was not created");
                
            return true;

        }

        public async Task<bool> EditBooking(string eventId, Booking newBooking)
        {
            //checks if selected time available to book
            var dateEvents = await GraphRepository.GetCalendarEventsByDate(
                newBooking.MeetingRoomName.ToString(), newBooking.StartTime, newBooking.EndTime);

            if (dateEvents.Count > 1 && dateEvents != null)
                throw new BusinessException("Selected room at this time is ocupied");

            if (dateEvents.Count == 1)
            {
                if(dateEvents[0].Id != eventId)
                {
                    var date = DateTime.Parse(dateEvents[0].End.DateTime);
                    if (date > newBooking.StartTime.ToUniversalTime())
                        throw new BusinessException("Selected room at this time is ocupied");
                }            
            }
            //----------------------------------------------------------

            var roomDetails = await GraphRepository.GetSelectedRoomCalendarDetails(newBooking.MeetingRoomName);
            var calendarEvent = await CreateEventFromBooking(newBooking, roomDetails);

            var updatedSuccessfully = await GraphRepository.EditEventById(eventId, calendarEvent);
            if (!updatedSuccessfully)
                throw new BusinessException("Something went wrong. Event was not updated");

            return updatedSuccessfully;
            // if(true)  Message: Booking successful
            // if(false) Message: Booking unsuccessful, choose another time or room
        }

        public async Task<bool> CancelBooking(string eventId, string roomName)
        {
            var isDeleted = await GraphRepository.DeleteEventById(eventId, roomName);

            if(!isDeleted)
                throw new BusinessException("Something went wrong. Event was not deleted");

            return true;
        }

        private async Task<Event> CreateEventFromBooking(Booking booking, RoomDetails roomDetails)
        {
            var participants = new List<Attendee>();
            foreach (var employeeId in booking.Participants)
            {
                var participantUser = await GraphRepository.GetOffice365UserById(employeeId);
                var participant = new Attendee
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = participantUser.Email,
                        Name = participantUser.FullName,
                    },
                    Type = AttendeeType.Required
                };
                participants.Add(participant);
            }
            var organizerUser = await GraphRepository.GetOffice365UserById(booking.Organizer);
            var calendatEvent = new Event
            {
                Subject = booking.Subject,
                Start = new DateTimeTimeZone
                {
                    TimeZone = TimeZoneInfo.Local.Id,
                    DateTime = booking.StartTime.ToString()
                },
                End = new DateTimeTimeZone
                {
                    TimeZone = TimeZoneInfo.Local.Id,
                    DateTime = booking.EndTime.ToString()
                },
                Location = new Location
                {
                    DisplayName = booking.MeetingRoomName,
                    LocationUri = roomDetails.Email,
                    LocationType = LocationType.ConferenceRoom,
                    UniqueIdType = LocationUniqueIdType.Directory
                },
                IsOrganizer = false,
                Organizer = new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Name = organizerUser.FullName,
                        Address = organizerUser.Email,
                    }
                },
                Attendees = participants
            };
            return calendatEvent;
        }
    }
}
