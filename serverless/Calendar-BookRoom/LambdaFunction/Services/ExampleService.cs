using System.Threading.Tasks;
using LambdaFunction.Settings;

namespace LambdaFunction.Services
{
    // Example service
    public class ExampleService : IExampleService
    {
        public async Task<string> GetHelloWorld()
        {
            return AppSettings.GetString("HelloWorld");
        }
    }
}