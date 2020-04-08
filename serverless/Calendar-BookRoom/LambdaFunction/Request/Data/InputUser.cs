using System;

namespace LambdaFunction.Inputs
{
    public class InputUser
    {
        public string Id { get; set; }
        
        public DateTime ModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public string Email { get; set; }

        public string DisplayName { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Meta { get; set; }
    }
}
