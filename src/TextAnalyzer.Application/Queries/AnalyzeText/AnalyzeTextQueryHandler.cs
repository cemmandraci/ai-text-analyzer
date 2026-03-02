using MediatR;
using TextAnalyzer.Application.DTOs;
using TextAnalyzer.Application.Exceptions;
using TextAnalyzer.Domain.Interfaces;

namespace TextAnalyzer.Application.Queries.AnalyzeText;

public class AnalyzeTextQueryHandler : IRequestHandler<AnalyzeTextQuery, AnalysisResultDto>
{
    private readonly INlpService _nlpService;
    private readonly ISentimentService _sentimentService;

    public AnalyzeTextQueryHandler(INlpService nlpService, ISentimentService sentimentService)
    {
        _nlpService = nlpService;
        _sentimentService = sentimentService;
    }

    public async Task<AnalysisResultDto> Handle(AnalyzeTextQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            throw new InvalidTextException("Text cannot be empty");

        if (request.Text.Length > 5000)
            throw new InvalidTextException("Text cannot exceed 5000 characters.");

        var nlpTask = _nlpService.AnalyzeAsync(request.Text, cancellationToken);
        var sentimentTask = _sentimentService.AnalyzeSentimentAsync(request.Text, cancellationToken);

        await Task.WhenAll(nlpTask, sentimentTask);
        
        var (keywords, entities) = await nlpTask;
        var (sentiment, score) = await sentimentTask;

        return new AnalysisResultDto
        {
            OriginalText = request.Text,
            Sentiment = sentiment.ToString(),
            SentimentScore = score,
            Keywords = keywords,
            Entities = entities.Select(e => new NamedEntityDto
            {
                Text = e.Text,
                Type = e.Type
            }).ToList(),
            AnalyzedAt = DateTime.UtcNow
        };
    }
}