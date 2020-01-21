using System;

namespace HrApp.Domain
{
    public class Commit
    {
        public Commit(EmployeeEntity employee, string description, double timeWorked)
        {
            Employee = employee;
            Description = description;
            TimeWorked = timeWorked;
            CommitDate = DateTime.Now;
        }

        public Commit()
        {
        }
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime CommitDate { get; set; }
        public EmployeeEntity Employee { get; set; }
        public double TimeWorked { get; set; }
    }
}
