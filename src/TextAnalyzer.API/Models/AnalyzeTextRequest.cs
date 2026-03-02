namespace TextAnalyzer.API.Models;

public class AnalyzeTextRequest
{
    public string Text { get; init; } = string.Empty;
    public string Language { get; init; } = "en";
}