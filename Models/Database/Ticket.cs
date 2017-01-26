using System;

namespace WebApplication.Models.Database {
    public class Ticket {
        public string TicketId { get; set; }
        public string Name { get; set; }

        public Ticket() {
            TicketId = Guid.NewGuid().ToString("D");
        }
    }    
}