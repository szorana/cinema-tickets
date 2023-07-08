using CinemaTickets.Domain.Domain;
using Microsoft.AspNetCore.Identity;

namespace CinemaTickets.Domain.Identity
{
    public class TicketShopApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Cart UserCart { get; set; }
    }
}
