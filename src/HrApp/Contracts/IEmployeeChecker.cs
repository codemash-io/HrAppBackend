namespace Meeting_Room_Booking_System.Booking_actions
{
    public interface IEmployeeChecker
    {
        bool EmployeesAreNotAttending(Employee[] employees); // Checks if booking employees are not attending another meeting at the same time
    }
}