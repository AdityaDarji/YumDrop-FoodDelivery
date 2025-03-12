using FoodDeliveryAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodDeliveryAPI.Services
{
     public class OrderServices
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderServices(IMongoDatabase database, IOptions<DatabaseSettings> settings)
        {
            _orderCollection = database.GetCollection<Order>(settings.Value.OrderCollectionName);
        }

        // Get all orders (Async)
        public async Task<List<Order>> GetOrdersAsync() =>
            await _orderCollection.Find(_ => true).ToListAsync();

        // Get order by ID (Async)
        public async Task<Order> GetOrderByIdAsync(string id) =>
            await _orderCollection.Find(order => order.Id == id).FirstOrDefaultAsync();

        // Create an order (Async)
        public async Task CreateOrderAsync(Order order) =>
            await _orderCollection.InsertOneAsync(order);

        // Update an order (Async)
        public async Task<bool> UpdateOrderAsync(string id, Order order)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);

            var update = Builders<Order>.Update
                .Set(o => o.CustomerName, order.CustomerName)
                .Set(o => o.FoodItem, order.FoodItem)
                .Set(o => o.Price, order.Price);

            var result = await _orderCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // Delete an order (Async)
        public async Task<bool> DeleteOrderAsync(string id)
        {
            var result = await _orderCollection.DeleteOneAsync(o => o.Id == id);
            return result.DeletedCount > 0;
        }
    }

}
