using System.Collections.Generic;
using CinemaTickets.Domain.Identity;
using CinemaTickets.Domain.Relations;

namespace CinemaTickets.Domain.Domain
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public TicketShopApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
