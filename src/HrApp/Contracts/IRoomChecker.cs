namespace HrApp
{
    public interface IRoomChecker
    {
        bool TimeLineIsEmpty(Booking booking); // Checks if room is available for booking
        bool RoomIsEmpty(Booking booking);
        bool BookingExists(Booking booking);   // Checks if booking exists and can be cancelled
    }
}