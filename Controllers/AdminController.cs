using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models;

namespace WebApplication.Controllers {
    [Authorize]
    public class AdminController : Controller {
        private readonly ITicketService _ticketService;

        public AdminController(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        public IActionResult Index(Int32 pageIndex = 0, Int32 pageSize = 20) {
            var model = new AdminIndexModel {
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            model.Tickets = _ticketService.GetOrders(pageIndex, pageSize);
            model.OrderedTickets = _ticketService.GetTicketCount();
            model.PaidTickets = _ticketService.GetPaidTicketCount();
            return View(model);
        }

        [HttpPut("/admin/api/orders/{orderId}/set-paid")]
        public IActionResult SetPaid(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Paid = true;
            order.PaidDate = DateTime.Now;
            _ticketService.Save(order);
            return new ObjectResult(order);
        }

        [HttpPut("/admin/api/orders/{orderId}/unset-paid")]
        public IActionResult SetUnpaid(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Paid = false;
            order.PaidDate = null;
            _ticketService.Save(order);
            return new ObjectResult(order);
        }

        [HttpDelete("/admin/api/orders/{orderId}")]
        public IActionResult RemoveOrder(string orderId) {
            var order = _ticketService.GetOrder(orderId);
            order.Deleted = true;
            _ticketService.Save(order);
            return NoContent();
        }
    }
}
