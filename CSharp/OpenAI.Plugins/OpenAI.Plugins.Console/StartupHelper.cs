namespace OpenAI.Plugins.Console;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Plugins.Helper;

/// <summary>
/// A helper class for building and retrieving services for OpenAI.Plugins.Console.
/// </summary>
public static class StartupHelper
{
    /// <summary>
    /// Builds and returns a collection of services for OpenAI.Plugins.Console.
    /// </summary>
    /// <returns>A ServiceCollection object containing the built services.</returns>
    public static ServiceCollection BuildServices()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile("appsettings.local.json", optional: true);
        IConfiguration configuration = configBuilder.Build();
        ServiceCollection services = new ServiceCollection();
        _ = services.AddOpenAIPluginsHelper(configuration);
        _ = services.BuildServiceProvider();
        return services;
    }

    /// <summary>
    /// Retrieves a service of type T from the provided collection of services.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <param name="services">The collection of services to retrieve the service from.</param>
    /// <returns>The retrieved service of type T.</returns>
    public static T GetService<T>(IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<T>();
        return service;
    }
}