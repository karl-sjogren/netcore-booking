using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models;

namespace WebApplication.Controllers {
    public class HomeController : Controller {
        private readonly ITicketService _ticketService;
        private readonly IAuthenticationStore _authenicationStore;

        public HomeController(ITicketService ticketService, IAuthenticationStore authenticationStore) {
            _ticketService = ticketService;
            _authenicationStore = authenticationStore;
        }

        public IActionResult Index([FromQuery]string auth = null) {
            var model = new HomeModel();
            model.Authenticated = auth == _authenicationStore.GetPin();
            model.TicketsLeft = 120 - _ticketService.GetTicketCount();
            return View(model);
        }

        public IActionResult Whopsie() {
            return View();
        }
    }
}
