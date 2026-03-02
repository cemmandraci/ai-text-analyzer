using TextAnalyzer.Domain.Enums;

namespace TextAnalyzer.Domain.Entities;

public class AnalysisResult
{
    public string OriginalText { get; init; } = string.Empty;
    public SentimentType Sentiment { get; set; }
    public double SentimentScore { get; set; }
    public IReadOnlyList<string> Keywords { get; set; } = [];
    public IReadOnlyList<NamedEntity> Entities { get; set; } = [];
    public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
}

public class NamedEntity
{
    public string Text { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty; // PERSON, ORG, GPE vs.
}