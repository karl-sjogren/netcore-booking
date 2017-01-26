using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models;

namespace WebApplication.Controllers {
    public class AdminController : Controller {
        private readonly ITicketService _ticketService;
        private readonly IAuthenticationStore _authenicationStore;

        public AdminController(ITicketService ticketService, IAuthenticationStore authenticationStore) {
            _ticketService = ticketService;
            _authenicationStore = authenticationStore;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet("/admin/api/orders")]
        public IActionResult Confirmation(Int32 pageIndex = 0, Int32 pageSize = 20) {
            var orders = _ticketService.GetOrders(pageIndex, pageSize);
            return new ObjectResult(orders);
        }
    }
}
