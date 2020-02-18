using CodeMash.Models;

namespace HrApp
{
    [Collection("Countries")]
    public class CountryEntity : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        
    }
}
