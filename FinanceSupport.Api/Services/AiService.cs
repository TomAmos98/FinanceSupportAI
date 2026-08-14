using OpenAI.Chat;

namespace FinanceSupport.Api.Services;

public class AiService
{
    private readonly ChatClient _chatClient;

    public AiService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API-nyckeln saknas.");
        }

        _chatClient = new ChatClient(
            model: "gpt-5-mini",
            apiKey: apiKey);
    }

    public async Task<string> GenerateAnswerAsync(
        string question,
        string context)
    {
        List<ChatMessage> messages =
        [
            new SystemChatMessage(
                """
                Du är en kundserviceassistent för ett företag inom finans.

                Svara endast utifrån informationen som finns i underlaget.
                Hitta inte på information.
                Om underlaget inte räcker för att besvara frågan ska du säga
                att kunden behöver kontakta kundservice.

                Ge korta, tydliga och professionella svar på svenska.
                """
            ),

            new UserChatMessage(
                $"""
                 Underlag:
                 {context}

                 Kundens fråga:
                 {question}
                 """
            )
        ];

        ChatCompletion completion =
            await _chatClient.CompleteChatAsync(messages);

        return completion.Content[0].Text;
    }
}