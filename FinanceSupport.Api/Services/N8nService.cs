using System.Net.Http.Json;

namespace FinanceSupport.Api.Services;

public class N8nService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public N8nService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendEscalatedQuestionAsync(string question)
    {
        var webhookUrl = _configuration["N8n:WebhookUrl"];

        if (string.IsNullOrWhiteSpace(webhookUrl))
        {
            return;
        }

        var payload = new
        {
            question
        };

        try
        {
            await _httpClient.PostAsJsonAsync(
                webhookUrl,
                payload);
        }
        catch
        {
            // n8n får inte krascha kundens API-flöde.
        }
    }
}