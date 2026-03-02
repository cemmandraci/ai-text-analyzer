using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TextAnalyzer.Domain.Enums;
using TextAnalyzer.Domain.Interfaces;
using TextAnalyzer.Infrastructure.Models;

namespace TextAnalyzer.Infrastructure.Services;

public class GeminiSentimentService : ISentimentService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonOptions;

    public GeminiSentimentService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini API key is not configured");
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<(SentimentType Sentiment, double Score)> AnalyzeSentimentAsync(string text, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("Gemini");

        var prompt = $@"
                Analyze the sentiment of the following text and respond ONLY with a JSON object.
                
                Text: ""{text}""
                
                Response format :
                {{
                    ""sentiment"": ""Positive"" | ""Negative"" | ""Neutral"" | ""Mixed"",
                    ""score"": ""<float between 0.0 and 1.0>"",
                    ""reasoning"": ""<brief explanation>""
                }}";

        var request = new GeminiRequest
        {
            Contents =
            [
                new GeminiContent
                {
                    Parts = [new GeminiPart
                    {
                        Text = prompt
                    }]
                }
            ],
            GenerationConfig = new GenerationConfig { ResponseMimeType = "application/json" }
        };
        
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var url = $"v1beta/models/gemini-2.0-flash:generateContent";
        client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);
        var response = await client.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseBody, _jsonOptions);
        
        var resultText = geminiResponse?.Candidates[0].Content.Parts[0].Text ?? throw new InvalidOperationException("Empty response from Gemini.");
        
        var result = JsonSerializer.Deserialize<SentimentAnalysisResult>(resultText, _jsonOptions) ?? throw new InvalidOperationException("Could not parse Gemini response");


        var sentimentType = result.Sentiment switch
        {
            "Positive" => SentimentType.Positive,
            "Negative" => SentimentType.Negative,
            "Mixed" => SentimentType.Mixed,
            _ => SentimentType.Neutral
        };

        return (sentimentType, result.Score);
    }
}