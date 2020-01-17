namespace Meeting_Room_Booking_System.Interfaces
{
    public interface IRoomBooker
    {
        bool BookRoom(Booking booking);
        bool EditBooking(Booking previousBooking, Booking newBooking);
        bool CancelBooking(Booking booking);
    }
}