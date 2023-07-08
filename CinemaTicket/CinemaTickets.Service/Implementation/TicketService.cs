using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CinemaTickets.Domain.Domain;
using CinemaTickets.Domain.DTO;
using CinemaTickets.Domain.Relations;
using CinemaTickets.Repository.Interface;
using CinemaTickets.Service.Interface;

namespace CinemaTickets.Service.Implenetation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInCart> _ticketInCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInCart> ticketInCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInCartRepository = ticketInCartRepository;
        }


        public bool AddToCart(AddToCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userCart = user.UserCart;

            if (item.SelectedTicketId != null && userCart != null)
            {
                var ticket = this.GetDetailsForTicket(item.SelectedTicketId);
                //{896c1325-a1bb-4595-92d8-08da077402fc}

                if (ticket != null)
                {
                    TicketInCart itemToAdd = new TicketInCart
                    {
                        Id = Guid.NewGuid(),
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        Cart = userCart,
                        CartId = userCart.Id,
                        Quantity = item.Quantity
                    };

                    var existing = userCart.TicketInCarts.Where(z => z.CartId == userCart.Id && z.TicketId == itemToAdd.TicketId).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.Quantity += itemToAdd.Quantity;
                        this._ticketInCartRepository.Update(existing);

                    }
                    else
                    {
                        this._ticketInCartRepository.Insert(itemToAdd);
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewTicket(Ticket p)
        {
            this._ticketRepository.Insert(p);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTicket()
        {
            return this._ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public AddToCartDto GetCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            AddToCartDto model = new AddToCartDto
            {
                SelectedTicket = ticket,
                SelectedTicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingTicket(Ticket p)
        {
            this._ticketRepository.Update(p);
        }

        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public List<Ticket> GetTicketsByGenre(string genre)
        {
            List<Ticket> tickets = GetAllTicket();
            tickets = tickets.FindAll(x => x.Genre.ToLower().StartsWith(genre));
            return tickets;
        }

        public List<Ticket> GetTicketsByValidDateAfter(DateTime validDate)
        {
            List<Ticket> tickets = GetAllTicket();
            tickets = tickets.FindAll(x => x.ValidDate > validDate);
            return tickets;
        }
    }
}
