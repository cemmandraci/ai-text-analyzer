namespace TextAnalyzer.Domain.Entities;

public class AnalysisRequest
{
    public string Text { get; init; } = string.Empty;
    public string Language { get; set; } = "en";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}