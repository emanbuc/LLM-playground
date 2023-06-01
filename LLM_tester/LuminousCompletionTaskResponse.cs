// See https://aka.ms/new-console-template for more information
using System.Text;

public class LuminousCompletionTaskResponse
{
    public List<CompletionResponse> Completions { get; set; }
    public string Model_version { get; set; }

    //constructors
    public LuminousCompletionTaskResponse()
    {
        Completions = new List<CompletionResponse>();
        Model_version = string.Empty;
    }

    internal string GetCompletionText()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in Completions)
        {
            sb.AppendLine(item.Completion);
        }
        return sb.ToString();
    }
}

public class CompletionResponse
{
    public string Completion { get; set; }
    public string Finish_reason { get; set; }

    public CompletionResponse()
    {
        Completion = string.Empty;
        Finish_reason = string.Empty;
    }

}