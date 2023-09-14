using static System.Formats.Asn1.AsnWriter;
using System.Numerics;
using System.Security.Policy;

namespace AOAI.Solution.Functions.Helpers;

public static class PromptHelper
{
    public static string GetPromptForEvaluation(string question, string answer, int fullmarks)
    {
        string prompt = $"As an examiner, your task is to rigorously evaluate a student's response to the following question: \"{question}\". The student's answer is as follows: \"{answer}\". \n Please assess the provided answer. Each statement of the answer must be factaully correct. Assign an overall evaluation score out of { fullmarks} to provide a detailed and constructive evaluation of the response. Consider factors such as factual correctness, depth of understanding, and supporting factually correct evidence while evaluating the answer.";
        return prompt;
    }
}
