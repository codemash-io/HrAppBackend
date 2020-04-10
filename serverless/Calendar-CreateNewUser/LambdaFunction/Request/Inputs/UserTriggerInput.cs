namespace LambdaFunction.Inputs
{
    public class UserTriggerInput : BasicInput
    {
        public string CollectionName { get; set; }
        
        public InputUser FormerUser { get; set; }

        public InputUser NewUser { get; set; }
        
        public string TriggerType { get; set; }
    }
}
