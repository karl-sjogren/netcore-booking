using System;
using System.Collections.Generic;
using WebApplication.Models.Database;

namespace WebApplication.Contracts {
    public interface ITicketService {
         Int32 GetTicketCount();
         TicketOrder GetOrder(string id);
         List<TicketOrder> GetOrders();
         void Save(TicketOrder order);
         void Remove(TicketOrder order);
    }
}