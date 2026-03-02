namespace TextAnalyzer.Infrastructure.Models;

public class GeminiRequest
{
    public List<GeminiContent> Contents { get; init; } = [];
    public GenerationConfig GenerationConfig { get; init; } = new();
}

public class GeminiContent
{
    public List<GeminiPart> Parts { get; init; } = [];
}

public class GeminiPart
{
    public string Text { get; init; } = string.Empty;
}

public class GenerationConfig
{
    public string ResponseMimeType { get; init; } = "application/json";
}