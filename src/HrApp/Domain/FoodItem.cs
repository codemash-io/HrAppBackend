using System.Collections.Generic;

namespace HrApp
{
    public class FoodItem
    {
        public string Name { get; set; }
        public List<EmployeeEntity> Employees { get; set; }
    }
}