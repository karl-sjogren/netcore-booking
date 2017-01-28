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

        /*[HttpGet("/admin/api/orders")]
        public IActionResult Confirmation() {
            var orders = _ticketService.GetOrders(pageIndex, pageSize);
            return new ObjectResult(orders);
        }*/
    }
}
