namespace AOAI.Solution.Helper.Abstractions;

public interface ISimilarityCalculator
{
    double CalculateCosimeSimilarity(List<float> embedding1, List<float> embedding2);

    double CalculateCosineDistance(List<float> embedding1, List<float> embedding2);
}
