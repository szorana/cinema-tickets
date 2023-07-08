using CinemaTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketShopApplicationUser> GetAll();
        TicketShopApplicationUser Get(string id);
        void Insert(TicketShopApplicationUser entity);
        void Update(TicketShopApplicationUser entity);
        void Delete(TicketShopApplicationUser entity);
    }
}
