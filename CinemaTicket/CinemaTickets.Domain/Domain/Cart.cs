using System;
using System.Collections.Generic;
using CinemaTickets.Domain;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Domain.Relations;

namespace CinemaTickets.Domain.Domain
{
    public class Cart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual TicketShopApplicationUser Owner { get; set; }

        public virtual ICollection<TicketInCart> TicketInCarts { get; set; }
    }
}
