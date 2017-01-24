using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Contracts;
using WebApplication.Models.Database;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Services {
    public class TicketService : ITicketService {
        private readonly IMongoDatabase _database;

        public TicketService(IConfigurationRoot configuration) {
            var connectionString = MongoUrl.Create(configuration["MONGODB_URI"]);
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(connectionString.DatabaseName);
        }

        public Int32 GetTicketCount() {
            var collection = _database.GetCollection<TicketOrder>("orders");
            var count = collection.AsQueryable().Select(order => order.TicketCount).Sum();
            return count;
        }

        public TicketOrder GetOrder(string id) {
            var collection = _database.GetCollection<TicketOrder>("orders");
            return collection.AsQueryable().FirstOrDefault(order => order.Id == id);
        }

        public List<TicketOrder> GetOrders() {
            var collection = _database.GetCollection<TicketOrder>("orders");
            return collection.AsQueryable().ToList();
        }
    }
}