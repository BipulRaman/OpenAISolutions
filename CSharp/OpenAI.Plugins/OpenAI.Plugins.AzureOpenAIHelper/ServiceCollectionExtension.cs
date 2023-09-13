namespace OpenAI.Plugins.Helper;

using Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Plugins.Helper.Abstractions;
using OpenAI.Plugins.Helper.Models;
using OpenAI.Plugins.Helper.Services;

/// <summary>
/// Provides extension methods to register OpenAI plugins helper services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Extension method to add OpenAI plugins helper services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    /// <returns>The <see cref="IServiceCollection"/> instance with the added services.</returns>
    public static IServiceCollection AddOpenAIPluginsHelper(this IServiceCollection services, IConfiguration configuration)
    {
        OpenAIConfiguration openAIConfiguration = configuration.GetSection(nameof(OpenAIConfiguration)).Get<OpenAIConfiguration>();
        _ = services.Configure<OpenAIConfiguration>(options =>
        {
            options.ApiKey = openAIConfiguration.ApiKey;
            options.CompletionModelDeploymentName = openAIConfiguration.CompletionModelDeploymentName;
            options.EmbeddingModelDeploymentName = openAIConfiguration.EmbeddingModelDeploymentName;
            options.Endpoint = openAIConfiguration.Endpoint;
        });
        _ = services.AddOpenAIClient(openAIConfiguration);
        _ = services.AddSingleton<ITextHelper, TextHelper>();

        return services;
    }

    /// <summary>
    /// Adds an OpenAI client to the specified <see cref="IServiceCollection"/> with the provided <see cref="OpenAIConfiguration"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the OpenAI client to.</param>
    /// <param name="openAIConfiguration">The <see cref="OpenAIConfiguration"/> containing the endpoint and API key for the OpenAI client.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddOpenAIClient(this IServiceCollection services, OpenAIConfiguration openAIConfiguration)
    {
        Uri endpoint = new Uri(openAIConfiguration.Endpoint);
        AzureKeyCredential azureKeyCredential = new AzureKeyCredential(openAIConfiguration.ApiKey);
        services.AddAzureClients(builder => builder.AddOpenAIClient(endpoint, azureKeyCredential).WithName(openAIConfiguration.EmbeddingModelDeploymentName));
        services.AddAzureClients(builder => builder.AddOpenAIClient(endpoint, azureKeyCredential).WithName(openAIConfiguration.CompletionModelDeploymentName));
        return services;
    }
}
