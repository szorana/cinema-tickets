using System.Collections.Generic;
using CinemaTickets.Domain.Relations;

namespace CinemaTickets.Domain.DTO
{
    public class CartDto
    {
        public List<TicketInCart> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
