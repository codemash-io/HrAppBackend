using System;
using System.Collections.Generic;
using Meeting_Room_Booking_System.Exceptions;

namespace Meeting_Room_Booking_System
{
    public class Employee
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public Employee()
        {
               
        }

        public Employee(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public override bool Equals(object otherEmployee)
        {
            if(!ReferenceEquals(otherEmployee, null))
                return Email.Equals(((Employee)otherEmployee).Email);

            throw new ArgumentNullException();
        }

        public static bool operator ==(Employee employee1, Employee employee2)
        {
            return employee1.Equals(employee2);
        }

        public static bool operator !=(Employee employee1, Employee employee2)
        {
            return !employee1.Equals(employee2);
        }

        public void CheckEmployeeProperties()
        {
            if(String.IsNullOrEmpty(Email))
                throw new EmployeePropertyException("Employee email is wrong or null");

            if(String.IsNullOrEmpty(Name))
                throw new EmployeePropertyException("Employee name property is null");
        }
    }
}