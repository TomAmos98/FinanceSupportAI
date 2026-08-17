using FinanceSupport.Api.Models;
using FinanceSupport.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSupport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<SupportTicket>> GetAll()
    {
        return Ok(_ticketService.GetAllTickets());
    }
}