namespace HrApp
{
    public interface IEmployeeChecker
    {
        bool EmployeesAreNotAttending(Employee[] employees); // Checks if booking employees are not attending another meeting at the same time
    }
}