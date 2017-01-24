using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models;

namespace WebApplication.Controllers {
    public class HomeController : Controller {
        private readonly ITicketService _ticketService;

        public HomeController(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        public IActionResult Index([FromQuery]string auth = null) {
            var model = new HomeModel();
            model.Authenticated = auth == "99RB";
            model.TicketsLeft = 120 - _ticketService.GetTicketCount();
            return View(model);
        }

        public IActionResult Whopsie() {
            return View();
        }
    }
}
