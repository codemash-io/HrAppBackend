using System.Threading.Tasks;

namespace LambdaFunction.Services
{
    // Example service interface
    public interface IExampleService
    {
        Task<string> GetHelloWorld();
    }
}