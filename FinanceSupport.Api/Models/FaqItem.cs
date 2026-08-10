namespace FinanceSupport.Api.Models;

public class FaqItem
{
    public string Title { get; set; } = string.Empty;
    public List<string> Keywords { get; set; } = [];
    public string Content { get; set; } = string.Empty;
}