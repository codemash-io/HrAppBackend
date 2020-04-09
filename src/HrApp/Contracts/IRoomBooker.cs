using System.Threading.Tasks;

namespace HrApp
{
    public interface IRoomBooker
    {
        Task<bool> BookRoom(Booking booking);
        Task<bool> EditBooking(string eventId, Booking newBooking);
        Task<bool> CancelBooking(string eventId, string roomName);
    }
}