namespace OpenAI.Plugins.Helper.Services;

using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using OpenAI.Plugins.Helper.Abstractions;
using OpenAI.Plugins.Helper.Models;

/// <summary>
/// Helper class for working with text-related functionality using OpenAI APIs.
/// </summary>
public class TextHelper : ITextHelper
{
    private readonly IAzureClientFactory<OpenAIClient> azureClientFactory;
    private readonly OpenAIConfiguration openAIConfiguration;

    public TextHelper(IAzureClientFactory<OpenAIClient> azureClientFactory, IOptionsMonitor<OpenAIConfiguration> openAIConfigurationOptions)
    {
        this.azureClientFactory = azureClientFactory;
        this.openAIConfiguration = openAIConfigurationOptions.CurrentValue;
    }

    /// <inheritdoc/>
    public async Task<List<float>> GetEmbeddingAsync(string texts, EmbeddingsOptions embeddingOptions = null)
    {
        List<float> outputEmbedding = new();

        if (embeddingOptions is null)
        {
            string sanitisedTexts = SanitizeTextForEmbeddingGeneration(texts);
            embeddingOptions = new(sanitisedTexts);
        }

        if (!string.IsNullOrWhiteSpace(texts))
        {
            OpenAIClient openAIClient = this.azureClientFactory.CreateClient(this.openAIConfiguration.EmbeddingModelDeploymentName);
            Response<Embeddings> output = await openAIClient.GetEmbeddingsAsync(this.openAIConfiguration.EmbeddingModelDeploymentName, embeddingOptions).ConfigureAwait(false);

            if (output is not null && output.Value.Data is not null)
            {
                outputEmbedding = output.Value.Data.Select(e => e.Embedding.ToList()).FirstOrDefault();
            }
        }

        return outputEmbedding;
    }

    /// <inheritdoc/>
    public async Task<List<List<float>>> GetEmbeddingsAsync(List<string> texts, EmbeddingsOptions embeddingOptions = null)
    {
        List<List<float>> outputEmbeddings = new();

        if (embeddingOptions is null)
        {
            List<string> sanitisedTexts = texts.Select(text => SanitizeTextForEmbeddingGeneration(text)).ToList();
            embeddingOptions = new(sanitisedTexts);
        }

        OpenAIClient openAIClient = this.azureClientFactory.CreateClient(this.openAIConfiguration.EmbeddingModelDeploymentName);
        Response<Embeddings> output = await openAIClient.GetEmbeddingsAsync(this.openAIConfiguration.EmbeddingModelDeploymentName, embeddingOptions).ConfigureAwait(false);

        if (output is not null && output.Value.Data is not null)
        {
            outputEmbeddings = output.Value.Data.Select(e => e.Embedding.ToList()).ToList();
        }

        return outputEmbeddings;
    }

    /// <inheritdoc/>
    public async Task<string> GetTextCompletionAsync(string promptText, CompletionsOptions completionsOptions = null)
    {
        if (completionsOptions is null)
        {
            IEnumerable<string> prompts = new List<string>() { promptText };
            completionsOptions = new CompletionsOptions(prompts)
            {
                Temperature = 1,
                MaxTokens = 2000,
            };
        }

        OpenAIClient openAIClient = this.azureClientFactory.CreateClient(this.openAIConfiguration.CompletionModelDeploymentName);

        Response<Completions> completionsResponse = await openAIClient.GetCompletionsAsync(this.openAIConfiguration.CompletionModelDeploymentName, completionsOptions).ConfigureAwait(false);
        string completionText = completionsResponse.Value.Choices[0].Text;

        return completionText;
    }

    /// <summary>
    /// Method to sanitize text for embedding generation.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string SanitizeTextForEmbeddingGeneration(string text)
    {
        return text.Replace("\n", " ", StringComparison.OrdinalIgnoreCase);
    }
}
