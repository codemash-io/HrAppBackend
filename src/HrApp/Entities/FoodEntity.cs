using CodeMash.Models;

namespace HrApp
{
    [Collection("Foods")]
    public class FoodEntity : Entity
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public FoodType FoodType { get; set; }
    }
}