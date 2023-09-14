namespace AOAI.Solution.Helper.Abstractions;

using Azure.AI.OpenAI;

/// <summary>
/// Provides methods for working with text, including getting text completions and embeddings.
/// </summary>
public interface ITextHelper
{
    ///<summary>
    ///Get text completion
    ///</summary>
    ///<param name="promptText">The prompt text to complete.</param>
    ///<param name="completionsOptions">The completions options.</param>
    ///<returns>The completed text.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "<Pending>")]
    Task<string> GetTextCompletionAsync(string promptText, CompletionsOptions completionsOptions = null);

    ///<summary>
    ///Get text embedding.
    ///</summary>
    ///<param name="texts">The text to embed in a vector.</param>
    ///<param name="embeddingOptions">The embedding options.</param>
    ///<returns>A list of floats representing the text embedding.</returns>
    Task<List<float>> GetEmbeddingAsync(string texts, EmbeddingsOptions embeddingOptions = null);

    ///<summary>
    ///Get text embeddings.
    ///</summary>
    ///<param name="texts">The text to embed in a vector.</param>
    ///<param name="embeddingOptions">The embedding options.</param>
    ///<returns>A list of lists of floats representing the text embeddings.</returns>
    Task<List<List<float>>> GetEmbeddingsAsync(List<string> texts, EmbeddingsOptions embeddingOptions = null);
}
