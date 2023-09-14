namespace AOAI.Solution.Helper.Services;

using AOAI.Solution.Helper.Abstractions;
using AOAI.Solution.Helper.Models;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;

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
        openAIConfiguration = openAIConfigurationOptions.CurrentValue;
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
            OpenAIClient openAIClient = azureClientFactory.CreateClient(openAIConfiguration.EmbeddingModelDeploymentName);
            Response<Embeddings> output = await openAIClient.GetEmbeddingsAsync(openAIConfiguration.EmbeddingModelDeploymentName, embeddingOptions).ConfigureAwait(false);

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

        OpenAIClient openAIClient = azureClientFactory.CreateClient(openAIConfiguration.EmbeddingModelDeploymentName);
        Response<Embeddings> output = await openAIClient.GetEmbeddingsAsync(openAIConfiguration.EmbeddingModelDeploymentName, embeddingOptions).ConfigureAwait(false);

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

        OpenAIClient openAIClient = azureClientFactory.CreateClient(openAIConfiguration.CompletionModelDeploymentName);

        Response<Completions> completionsResponse = await openAIClient.GetCompletionsAsync(openAIConfiguration.CompletionModelDeploymentName, completionsOptions).ConfigureAwait(false);
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
