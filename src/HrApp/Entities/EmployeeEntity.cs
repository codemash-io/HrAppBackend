using CodeMash.Models;

namespace HrApp
{
    [CollectionName("pc-employees")]
    public class EmployeeEntity : Entity
    {
        [FieldName("first_name")]
        public string FirstName { get; set; }
        
        [FieldName("last_name")]
        public string LastName { get; set; }
        
        [FieldName("division")]
        public string Division { get; set; }
    }
}
