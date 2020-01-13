using System;

namespace HrApp.Domain
{
    public class Commit
    {
        public Commit(EmployeeEntity employee, string description, TimeSpan timeWorked)
        {
            Employee = employee;
            Description = description;
            TimeWorked = timeWorked;
            CommitDate = DateTime.Now;  
        }

        public string Description { get; set; }
        public DateTime CommitDate { get; set; }
        public EmployeeEntity Employee { get; set; }
        public TimeSpan TimeWorked { get; set; }
    }
}
