using TextAnalyzer.Domain.Entities;

namespace TextAnalyzer.Domain.Interfaces;

public interface INlpService
{
    Task<(IReadOnlyList<string> Keywords, IReadOnlyList<NamedEntity> Entities)> AnalyzeAsync(string text, CancellationToken cancellationToken = default);
}