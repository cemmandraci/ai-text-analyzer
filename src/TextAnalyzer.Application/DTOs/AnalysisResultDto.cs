namespace TextAnalyzer.Application.DTOs;

public class AnalysisResultDto
{
    public string OriginalText { get; init; } = string.Empty;
    public string Sentiment { get; init; } = string.Empty;
    public double SentimentScore { get; init; }
    public IReadOnlyList<string> Keywords { get; init; } = [];
    public IReadOnlyList<NamedEntityDto> Entities { get; init; } = [];
    public DateTime AnalyzedAt { get; init; }
}

public class NamedEntityDto
{
    public string Text { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}