using FinanceSupport.Api.Models;
using FinanceSupport.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSupport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly KnowledgeService _knowledgeService;
    private readonly AiService _aiService;

    public ChatController(
        KnowledgeService knowledgeService,
        AiService aiService)
    {
        _knowledgeService = knowledgeService;
        _aiService = aiService;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Ask(
        [FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new ChatResponse
            {
                Answer = "Frågan får inte vara tom.",
                Source = null,
                Escalated = true
            });
        }

        var faqItem =
            _knowledgeService.FindRelevantInformation(request.Message);

        if (faqItem is null)
        {
            return Ok(new ChatResponse
            {
                Answer = "Jag kan inte besvara den frågan utifrån den information jag har. Kontakta kundservice för hjälp.",
                Source = null,
                Escalated = true
            });
        }

        var aiAnswer = await _aiService.GenerateAnswerAsync(
            request.Message,
            faqItem.Content);

        return Ok(new ChatResponse
        {
            Answer = aiAnswer,
            Source = faqItem.Title,
            Escalated = false
        });
    }
}