using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models.Database;

namespace WebApplication.Controllers {
    public class ApiController : Controller {
       private readonly ITicketService _ticketService;
        private readonly IAuthenticationStore _authenicationStore;

        public ApiController(ITicketService ticketService, IAuthenticationStore authenticationStore) {
            _ticketService = ticketService;
            _authenicationStore = authenticationStore;
        }

        private bool IsAuthenticated =>  Request.Query["auth"] == _authenicationStore.GetPin();

        [HttpPost("/api/orders")]
        public IActionResult PostOrder([FromBody]TicketOrder order) {
            if(!IsAuthenticated)
                return Unauthorized();

            _ticketService.Save(order);
            return NoContent();
        }
    }
}
