namespace LambdaFunction.Inputs
{
    /// <summary>
    /// This class should be used as a parameter in the main function to receive
    /// data from CodeMash
    /// </summary>
    /// <typeparam name="T">
    /// Custom type depending of how this function was called.
    /// Can be one of types from Inputs folder
    /// </typeparam>
    public class CustomEventRequest<T> where T: BasicInput
    {
        public string ProjectId { get; set; }
        
        public T Input { get; set; }
    }
}
