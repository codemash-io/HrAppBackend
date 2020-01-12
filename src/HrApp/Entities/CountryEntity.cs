using CodeMash.Models;

namespace HrApp
{
    [CollectionName("Countries")]
    public class CountryEntity : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        
    }
}
