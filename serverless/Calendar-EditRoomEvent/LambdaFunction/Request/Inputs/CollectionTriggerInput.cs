namespace LambdaFunction.Inputs
{
    public class CollectionTriggerInput : BasicInput
    {
        public string CollectionName { get; set; }
        
        public string FormerRecord { get; set; }
        
        public string NewRecord { get; set; }
        
        public string TriggerType { get; set; }
    }
}
