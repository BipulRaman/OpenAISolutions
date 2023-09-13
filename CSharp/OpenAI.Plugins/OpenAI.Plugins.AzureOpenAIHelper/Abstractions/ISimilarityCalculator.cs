namespace OpenAI.Plugins.AzureOpenAIHelper.Abstractions;

public interface ISimilarityCalculator
{
    double CalculateCosimeSimilarity(double[] embedding1, double[] embedding2);

    double CalculateCosineDistance(double[] embedding1, double[] embedding2);
}
