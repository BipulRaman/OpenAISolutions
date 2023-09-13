using OpenAI.Plugins.AzureOpenAIHelper.Abstractions;

namespace OpenAI.Plugins.AzureOpenAIHelper.Services;

public class SimilarityCalculator : ISimilarityCalculator
{
    public double CalculateCosimeSimilarity(double[] embedding1, double[] embedding2)
    {
        if (embedding1.Length != embedding2.Length)
        {
            return 0;
        }

        double dotProduct = 0.0;
        double magnitude1 = 0.0;
        double magnitude2 = 0.0;

        for (int i = 0; i < embedding1.Length; i++)
        {
            dotProduct += embedding1[i] * embedding2[i];
            magnitude1 += Math.Pow(embedding1[i], 2);
            magnitude2 += Math.Pow(embedding2[i], 2);
        }

        magnitude1 = Math.Sqrt(magnitude1);
        magnitude2 = Math.Sqrt(magnitude2);

        if (magnitude1 == 0.0 || magnitude2 == 0.0)
        {
            throw new ArgumentException
                 ("embedding must not have zero magnitude.");
        }

        double cosineSimilarity = dotProduct / (magnitude1 * magnitude2);

        return cosineSimilarity;
    }

    public double CalculateCosineDistance(double[] embedding1, double[] embedding2)
    {
        double cosineSimilarity = CalculateCosimeSimilarity(embedding1, embedding2);
        double cosineDistance = 1 - cosineSimilarity;

        return cosineDistance;
    }
}
