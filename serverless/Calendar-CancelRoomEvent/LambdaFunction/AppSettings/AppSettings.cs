using System.IO;
using Microsoft.Extensions.Configuration;

namespace LambdaFunction.Settings
{
    public static class AppSettings
    {
        private static IConfigurationRoot instance;

        public static string GetString(string key)
        {
            if (instance == null)
            {
                instance = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            
            return instance[key];
        }
    }
}
