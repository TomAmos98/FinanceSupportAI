namespace FinanceSupport.Api.Services;

public class AiService
{
    public Task<string> GenerateAnswerAsync(string question, string context)
    {
        var answer =
            $"Fråga: {question}\n\nUnderlag: {context}";

        return Task.FromResult(answer);
    }
}