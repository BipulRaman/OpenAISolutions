namespace OpenAI.Plugins.Console;

using Microsoft.Extensions.DependencyInjection;
using OpenAI.Plugins.Helper.Abstractions;
using System;

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        ServiceCollection services = StartupHelper.BuildServices();

        var openAiTextHelper = StartupHelper.GetService<ITextHelper>(services);
        var result = await openAiTextHelper.GetTextCompletionAsync("Where is the capital of England?").ConfigureAwait(false);
        var embeddings = await openAiTextHelper.GetEmbeddingAsync("Hello World").ConfigureAwait(false);
    }
}