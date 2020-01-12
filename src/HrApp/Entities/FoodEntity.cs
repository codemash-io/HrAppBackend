using CodeMash.Models;

namespace HrApp
{
    [CollectionName("Foods")]
    public class FoodEntity : Entity
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public FoodType FoodType { get; set; }
    }
}