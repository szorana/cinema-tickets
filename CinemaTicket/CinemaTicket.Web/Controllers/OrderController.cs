using CinemaTickets.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace CinemaTicket.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._orderService.getAllOrdersFromUser(userId);

            return View(result);
        }

        public IActionResult Details(Guid id)
        {
            var result = this._orderService.getOrderDetails(id);

            return View(result);
        }

        public IActionResult CreateInvoice(Guid id)
        {
            var result = this._orderService.getOrderDetails(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{Order Number}}", result.Id.ToString());
            document.Content.Replace("{{CustumerEmail}}", result.User.Email);
            document.Content.Replace("{{CustumerInfo}}", (result.User.FirstName + " " + result.User.LastName));

            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            foreach (var item in result.TicketInOrders)
            {
                total += item.Quantity * item.Ticket.Price;
                sb.AppendLine(item.Ticket.MovieName + " with quantity of: " + item.Quantity + " and price of: $" + item.Ticket.Price);
            }

            document.Content.Replace("{{AllTickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + total.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "Factura.pdf");
        }
    }
}
