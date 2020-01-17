namespace HrApp
{
    public interface IRoomBooker
    {
        bool BookRoom(Booking booking);
        bool EditBooking(Booking previousBooking, Booking newBooking);
        bool CancelBooking(Booking booking);
    }
}