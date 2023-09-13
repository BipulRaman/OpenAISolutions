namespace OpenAI.Plugins.Helper.Models;

/// <summary>
/// Represents the configuration required to access OpenAI API.
/// </summary>
public class OpenAIConfiguration
{
    /// <summary>
    /// Gets or sets the name of the endpoint.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the api key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the completion model deployment name.
    /// </summary>
    public string CompletionModelDeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the embedding model deployment name.
    /// </summary>
    public string EmbeddingModelDeploymentName { get; set; } = string.Empty;

    /// <summary>
    /// Validates this instance.
    /// </summary>
    /// <returns>Validation result.</returns>
    public bool Validate() =>
        !string.IsNullOrWhiteSpace(Endpoint) &&
        !string.IsNullOrWhiteSpace(ApiKey) &&
        !string.IsNullOrWhiteSpace(CompletionModelDeploymentName) &&
        !string.IsNullOrWhiteSpace(EmbeddingModelDeploymentName);
}
