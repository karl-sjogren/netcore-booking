using System;

namespace WebApplication {
    public class TicketException : Exception {
        public TicketException(string message) : base(message) { }
    }
}