using CinemaTickets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface ICartService
    {
        CartDto getCartInfo(string userId);
        bool deleteTicketFromCart(string userId, Guid ticketId);
        bool order(string userId);
    }
}
