using CodeMash.Models;

namespace HrApp
{
    [Collection("pc-employees")]
    public class EmployeeEntity : Entity
    {
        [Field("first_name")]
        public string FirstName { get; set; }
        
        [Field("last_name")]
        public string LastName { get; set; }
        
        [Field("division")]
        public string Division { get; set; }
    }
}
