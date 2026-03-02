using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TextAnalyzer.Domain.Interfaces;
using TextAnalyzer.Infrastructure.Services;

namespace TextAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Gemini", client =>
        {
            client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient("PythonNlp", client =>
        {
            client.BaseAddress = new Uri(configuration["PythonNlp:BaseUrl"] ?? "http://localhost:8000");
            client.Timeout = TimeSpan.FromSeconds(15);
        });

        services.AddScoped<ISentimentService, GeminiSentimentService>();
        services.AddScoped<INlpService, PythonNlpService>();

        return services;
    }
}