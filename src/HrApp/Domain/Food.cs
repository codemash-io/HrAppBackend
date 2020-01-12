using System.Collections.Generic;

namespace HrApp
{
    public class FoodOrder
    {
        public List<FoodItem> Items { get; set; } = new List<FoodItem>();
    }

    public class PersonalOrderPreference
    {
        public FoodType Type { get; set; }
        public int FoodNumber { get; set; }
        
    }
}
