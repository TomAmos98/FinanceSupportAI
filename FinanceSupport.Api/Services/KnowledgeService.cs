using System.Text.Json;
using FinanceSupport.Api.Models;

namespace FinanceSupport.Api.Services;

public class KnowledgeService
{
    private readonly List<FaqItem> _faqItems;

    public KnowledgeService()
    {
        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "faq.json"
        );

        if (!File.Exists(filePath))
        {
            _faqItems = [];
            return;
        }

        var json = File.ReadAllText(filePath);

        _faqItems = JsonSerializer.Deserialize<List<FaqItem>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        ) ?? [];
    }

    public FaqItem? FindRelevantInformation(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return null;
        }

        var normalizedQuestion = question.ToLowerInvariant();

        return _faqItems
            .Select(item => new
            {
                Item = item,
                Score = item.Keywords.Count(keyword =>
                    normalizedQuestion.Contains(keyword.ToLowerInvariant()))
            })
            .Where(result => result.Score > 0)
            .OrderByDescending(result => result.Score)
            .Select(result => result.Item)
            .FirstOrDefault();
    }
}