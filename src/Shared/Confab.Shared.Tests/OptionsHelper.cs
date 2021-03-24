using Microsoft.Extensions.Configuration;

namespace Confab.Shared.Tests
{
    public static class OptionsHelper
    {
        public static TSettings GetOptions<TSettings>(string section, string settingsFileName = null) where TSettings : class, new()
        {
            settingsFileName ??= "appsettings.test.json";
            var configuration = new TSettings();
            
            GetConfigurationRoot(settingsFileName)
                .GetSection(section)
                .Bind(configuration);

            return configuration;
        }
        
        public static IConfigurationRoot GetConfigurationRoot(string settingsFileName = "appsettings.test.json")
            => new ConfigurationBuilder()
                .AddJsonFile(settingsFileName, optional: true)
                .AddEnvironmentVariables()
                .Build();
    }
}