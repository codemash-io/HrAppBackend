using CodeMash.Models;
using System;

namespace HrApp
{
    [Collection("employees")]
    public class EmployeeEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; }        
        [Field("last_name")]
        public string LastName { get; set; }     
        [Field("division")]
        public string Division { get; set; }
        [Field("business_email")]
        public string Email { get; set; }


        /*public override bool Equals(object otherEmployee)
        {
            if (!ReferenceEquals(otherEmployee, null))
                return Email.Equals(((Employee)otherEmployee).Email);

            throw new ArgumentNullException();
        }*/

        public void CheckEmployeeProperties()
        {
            if (String.IsNullOrEmpty(Email))
                throw new EmployeePropertyException("Employee email is wrong or null");
            if (String.IsNullOrEmpty(FirstName))
                throw new EmployeePropertyException("Employee name property is null");
            if (String.IsNullOrEmpty(LastName))
                throw new EmployeePropertyException("Employee name property is null");
        }
    }
}
