using FinanceSupport.Api.Models;

namespace FinanceSupport.Api.Services;

public class TicketService
{
    private readonly List<SupportTicket> _tickets = [];

    public SupportTicket CreateTicket(string question)
    {
        var ticket = new SupportTicket
        {
            Question = question
        };

        _tickets.Add(ticket);

        return ticket;
    }

    public IReadOnlyList<SupportTicket> GetAllTickets()
    {
        return _tickets;
    }
}