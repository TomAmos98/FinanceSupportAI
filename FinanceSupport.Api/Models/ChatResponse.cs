namespace FinanceSupport.Api.Models;

public class ChatResponse
{
    public string Answer { get; set; } = string.Empty;
    public string? Source { get; set; }
    public bool Escalated { get; set; }
}