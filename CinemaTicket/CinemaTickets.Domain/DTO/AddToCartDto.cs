using System;
using CinemaTickets.Domain.Domain;

namespace CinemaTickets.Domain.DTO
{
    public class AddToCartDto
    {
        public Ticket SelectedTicket { get; set; }
        public Guid SelectedTicketId { get; set; }
        public int Quantity { get; set; }
    }
}
