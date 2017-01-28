using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Contracts;
using WebApplication.Models.Database;

namespace WebApplication.Controllers {
    public class ApiController : Controller {
        private readonly ITicketService _ticketService;
        private readonly IAuthenticationStore _authenicationStore;
        private readonly IMailService _mailService;

        public ApiController(ITicketService ticketService, IAuthenticationStore authenticationStore, IMailService mailService) {
            _ticketService = ticketService;
            _authenicationStore = authenticationStore;
            _mailService = mailService;
        }

        private bool IsAuthenticated => _authenicationStore.ValidatePin(Request.Query["auth"]);

        [HttpPost("/api/orders")]
        public IActionResult PostOrder([FromBody]TicketOrder order) {
            if(!IsAuthenticated)
                return Forbid();

            var ticketCount = _ticketService.GetTicketCount();

            if(ticketCount >= 120)
                return Forbid();

            var validator = new TicketOrderValidator();
            var result = validator.Validate(order);
            if(!result.IsValid)
                return BadRequest(result.Errors);

            order.TicketCount = order.Tickets.Count;

            _ticketService.Save(order);

            var builder = new StringBuilder();

            builder.AppendLine($"Hej {order.Name}!");
            builder.AppendLine();
            builder.AppendLine($"Tack för din bokning av {order.TicketCount} biljetter till Suntripfesten! Man måste ju betala också har jag hört, vi skickar ut info om det inom några dagar.");
            builder.AppendLine("Nedan följer en summering av din bokning.");
            
            builder.AppendLine();

            builder.AppendLine($"  Namn: {order.Name}");
            builder.AppendLine($"  Adress: {order.Address}");
            builder.AppendLine($"  Postnummer: {order.ZipCode}");
            builder.AppendLine($"  Stad: {order.City}");
            builder.AppendLine($"  Mobilnummer: {order.Phone}");
            builder.AppendLine($"  E-post: {order.Email}");
            
            builder.AppendLine();

            builder.AppendLine("Biljetter är bokade för följande namn.");
            foreach(var ticket in order.Tickets)
                builder.AppendLine("  " + ticket.Name);

            builder.AppendLine();
            builder.AppendLine("Har du frågor kan du kontakta oss genom att svara på detta mail.");
            builder.AppendLine();
            builder.AppendLine("/ Suntrip-folket");

            _mailService.SendMessage(order.Email, "Din bokning av Suntripfesten", builder.ToString());

            return NoContent();
        }
    }
}
