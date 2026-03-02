namespace TextAnalyzer.Infrastructure.Models;

public class NlpServiceResponse
{
    public List<string> Keywords { get; init; } = [];
    public List<NlpEntity> Entities { get; init; } = [];
}

public class NlpEntity
{
    public string Text { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
}