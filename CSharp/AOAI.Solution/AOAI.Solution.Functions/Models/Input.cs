namespace AOAI.Solution.Functions.Models;

public class Input
{
    public string Question { get; set; }
    public int Fullmarks { get; set; }
    public string Answer { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Question) || string.IsNullOrEmpty(Answer) || Fullmarks <= 0)
        {
            return false;
        }

        return true;
    }   
}
