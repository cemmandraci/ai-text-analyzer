using System.Text;
using System.Text.Json;
using TextAnalyzer.Domain.Entities;
using TextAnalyzer.Domain.Interfaces;
using TextAnalyzer.Infrastructure.Models;

namespace TextAnalyzer.Infrastructure.Services;

public class PythonNlpService : INlpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions;


    public PythonNlpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<(IReadOnlyList<string> Keywords, IReadOnlyList<NamedEntity> Entities)> AnalyzeAsync(string text, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("PythonNlp");
        
        var requestBody = JsonSerializer.Serialize(new {text}, _jsonOptions);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/analyze", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<NlpServiceResponse>(responseBody, _jsonOptions) ?? throw new InvalidOperationException("Could not parse NLP service response");

        var entities = result.Entities
            .Select(e => new NamedEntity { Text = e.Text, Type = e.Type })
            .ToList();
        
        return (result.Keywords, entities);
    }
    
}