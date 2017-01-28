using System;
using System.Collections.Generic;
using WebApplication.Models.Database;

namespace WebApplication.Models {
    public class AdminIndexModel {
        public Int32 OrderedTickets { get; set; }
        public Int32 PaidTickets { get; set; }
        public List<TicketOrder> Tickets { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 PageIndex { get; set; }
    }    
}