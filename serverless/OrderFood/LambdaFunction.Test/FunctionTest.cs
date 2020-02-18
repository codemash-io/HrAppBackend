<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xunit;
using Amazon.Lambda.TestUtilities;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using Moq;
using Newtonsoft.Json.Linq;

namespace LambdaFunction.Tests
{
    public class FunctionTest
    {
        private readonly Mock<IExampleService> exampleServiceMock;
        
        public FunctionTest()
        {
            exampleServiceMock = new Mock<IExampleService>();
            exampleServiceMock.Setup(m => m.GetHelloWorld()).Returns(Task.FromResult("Hello World"));
        }
        
        [Fact]
        public async Task Handler_can_return_response()
        {
            // Init function
            var function = new Function(exampleServiceMock.Object);

            // Get response from function
            var result = await function.Handler(new CustomEventRequest<BasicInput>(), new TestLambdaContext());
            var resultBody = JsonConvert.DeserializeObject<JObject>(result.Body);

            // Assert
            Assert.Equal("Hello World", resultBody["helloWorldMessage"].ToString());
            Assert.Equal("First nested parameter.", resultBody["nestedSettings"].ToString());
        }
    }
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xunit;
using Amazon.Lambda.TestUtilities;
using LambdaFunction.Inputs;
using LambdaFunction.Services;
using Moq;
using Newtonsoft.Json.Linq;

namespace LambdaFunction.Tests
{
    public class FunctionTest
    {
        private readonly Mock<IExampleService> exampleServiceMock;
        
        public FunctionTest()
        {
            exampleServiceMock = new Mock<IExampleService>();
            exampleServiceMock.Setup(m => m.GetHelloWorld()).Returns(Task.FromResult("Hello World"));
        }
        
        [Fact]
        public async Task Handler_can_return_response()
        {
            // Init function
            var function = new Function(exampleServiceMock.Object);

            // Get response from function
            var result = await function.Handler(new CustomEventRequest<BasicInput>(), new TestLambdaContext());
            var resultBody = JsonConvert.DeserializeObject<JObject>(result.Body);

            // Assert
            Assert.Equal("Hello World", resultBody["helloWorldMessage"].ToString());
            Assert.Equal("First nested parameter.", resultBody["nestedSettings"].ToString());
        }
    }
>>>>>>> master
}