using System;
using System.Collections.Generic;
using WebApplication.Models.Database;

namespace WebApplication.Contracts {
    public interface ITicketService {
         Int32 GetOrderCount();
         Int32 GetTicketCount();
         Int32 GetPaidTicketCount();
         TicketOrder GetOrder(string id);
         List<TicketOrder> GetOrders(Int32 pageIndex, Int32 pageSize);
         void Save(TicketOrder order);
         void Remove(TicketOrder order);
    }
}