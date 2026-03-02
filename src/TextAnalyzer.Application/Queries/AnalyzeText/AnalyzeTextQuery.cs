using MediatR;
using TextAnalyzer.Application.DTOs;

namespace TextAnalyzer.Application.Queries.AnalyzeText;

public record AnalyzeTextQuery(string Text, string Language = "en") : IRequest<AnalysisResultDto>;