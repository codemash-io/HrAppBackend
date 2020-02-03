namespace HrApp
{
    public class RoomBookerService : IRoomBooker
    {
        public IRoomChecker RoomChecker { get; set; }
        public IHrService HrService { get; set; }

        public RoomBookerService(IRoomChecker roomChecker, IHrService hrService)
        {
            RoomChecker = roomChecker;
            HrService = hrService;
        }

        public bool BookRoom(Booking booking)
        {
            bool timeLineIsEmpty = RoomChecker.TimeLineIsEmpty(booking);
            bool wantedRoomIsEmpty = RoomChecker.RoomIsEmpty(booking);

            return timeLineIsEmpty || wantedRoomIsEmpty;

            // if(true)  Message: Booking successful
            // if(false) Message: Booking unsuccessful, choose another time or room
        }

        public bool EditBooking(Booking booking, Booking newBooking)
        {
            bool timeLineIsEmpty = RoomChecker.TimeLineIsEmpty(newBooking);
            bool wantedRoomIsEmpty = RoomChecker.RoomIsEmpty(newBooking);

            if (booking.StartTime != newBooking.StartTime || booking.EndTime != newBooking.EndTime)
            {
                if (timeLineIsEmpty)
                    return true; // Message: Booking successful
                else
                {
                    return wantedRoomIsEmpty;
                    // if(true)  Message: Booking successful
                    // if(false) Message: Booking unsuccessful, choose another time or room
                }
            }

            return true;
        }

        public bool CancelBooking(Booking booking)
        {
            bool bookingExists = RoomChecker.BookingExists(booking);

            return bookingExists;
        }
    }
}
