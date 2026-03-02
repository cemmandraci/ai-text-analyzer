using TextAnalyzer.Domain.Enums;

namespace TextAnalyzer.Domain.Interfaces;

public interface ISentimentService
{
    Task<(SentimentType Sentiment, double Score)> AnalyzeSentimentAsync(string text, CancellationToken cancellationToken = default);
}