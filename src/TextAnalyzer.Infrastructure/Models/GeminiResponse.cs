namespace TextAnalyzer.Infrastructure.Models;

public class GeminiResponse
{
    public List<GeminiCandidate> Candidates { get; init; } = [];
}

public class GeminiCandidate
{
    public GeminiContent Content { get; init; } = new();
}

public class SentimentAnalysisResult
{
    public string Sentiment { get; init; } = string.Empty;
    public double Score { get; init; }
    public string Reasoning { get; init; } = string.Empty;
}