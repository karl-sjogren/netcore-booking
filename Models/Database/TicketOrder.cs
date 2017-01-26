using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace WebApplication.Models.Database {
    public class TicketOrder {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string TicketType { get; set; }
        
        public bool Paid { get; set;}
        public bool Deleted { get; set;}

        public DateTime OrderDate { get; set; }
        public DateTime? PaidDate { get; set; }

        public Int32 TicketCount { get; set; }
        public List<Ticket> Tickets { get; set; }

        public TicketOrder() {
            Id = ObjectId.GenerateNewId().ToString();
            OrderDate = DateTime.Now;
            Tickets = new List<Ticket>();
        }
    }    
}