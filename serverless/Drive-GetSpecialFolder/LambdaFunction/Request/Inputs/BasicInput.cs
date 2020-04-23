namespace LambdaFunction.Inputs
{
    /// <summary>
    /// Base input type. Used when invoking a function through API, testing, scheduler.
    /// </summary>
    public class BasicInput
    {
        /// <summary>
        /// Id of a user who invoked the function. Not always set.
        /// </summary>
        public string InitiatorUserId { get; set; }
        
        /// <summary>
        /// Your custom JSON template that you defined in a function
        /// </summary>
        public string Template { get; set; }
        
        /// <summary>
        /// Data passed to API request when calling a function
        /// </summary>
        public string Data { get; set; }
    }
}
