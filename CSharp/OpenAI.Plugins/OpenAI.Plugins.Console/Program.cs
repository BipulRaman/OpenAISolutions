namespace OpenAI.Plugins.Console;

using Microsoft.Extensions.DependencyInjection;
using OpenAI.Plugins.AzureOpenAIHelper.Abstractions;
using System;

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        ServiceCollection services = StartupHelper.BuildServices();

        var openAiTextHelper = StartupHelper.GetService<ITextHelper>(services);
        var similarityCalculator = StartupHelper.GetService<ISimilarityCalculator>(services);

        // Example of getting text generation.
        string result = await openAiTextHelper.GetTextCompletionAsync("Where is the capital of England?").ConfigureAwait(false);
        Console.WriteLine(result);


        // Example of calculating cosine similarity of texts.
        List<float> embedding1 = await openAiTextHelper.GetEmbeddingAsync("Hello World").ConfigureAwait(false);
        List<float> embedding2 = await openAiTextHelper.GetEmbeddingAsync("Hello Earth").ConfigureAwait(false);

        double similarityScore = similarityCalculator.CalculateCosimeSimilarity(embedding1, embedding2);
        Console.WriteLine($"Similarity score of texts: {similarityScore}");
    }
}