namespace LambdaFunction.Inputs
{
    public class FileTriggerInput : BasicInput
    {
        public string CollectionName { get; set; }
        
        public InputFile FormerFile { get; set; }

        public InputFile NewFile { get; set; }
        
        public string TriggerType { get; set; }
    }
}
