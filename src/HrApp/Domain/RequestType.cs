using CodeMash.Models;

namespace HrApp
{
    [Collection("request_types")]
    public class RequestType : Entity
    {
        [Field("request_type")]
        public string Type { get; private set; }

        public RequestType(string type)
        {
            Type = type;
        }
    }
}