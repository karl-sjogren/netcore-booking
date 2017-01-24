using System;
using System.Collections.Generic;

namespace WebApplication.Models.Database {
    public class TicketOrder {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        
        public bool Paid { get; set;}

        public Int32 TicketCount { get; set; }
        public List<Ticket> Tickets { get; }

        public TicketOrder() {
            Tickets = new List<Ticket>();
        }
    }    
}