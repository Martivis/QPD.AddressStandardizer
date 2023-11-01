using System.Reflection;

namespace QPD.AddressStandardizer.Services
{
    public static class SettingsLoader
    {
        public static T Load<T>(string key) where T : new()
        {
            var assembly = Assembly.GetAssembly(typeof(T))!;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(assembly)
                .Build();

            var settings = new T();

            configuration.GetSection(key).Bind(settings);
            return settings;
        }
    }
}
