using CodeMash.Models;

namespace HrApp
{
    [CollectionName("food_supplier")]
    public class FoodSupplierEntity : Entity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        [FieldName("image")]
        public string ImageId { get; set; }
    }
}
