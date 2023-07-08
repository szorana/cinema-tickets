using CinemaTickets.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTickets.Service.Interface
{
    public interface IOrderService
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid id);

        public List<Order> getAllOrdersFromUser(String userId);
    }
}
