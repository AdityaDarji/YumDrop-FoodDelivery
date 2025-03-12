using FoodDeliveryAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodDeliveryAPI.Services
{
    public class RestaurantServices
    {
        private readonly IMongoCollection<Restaurant> _restaurantCollection;

        public RestaurantServices(IMongoDatabase database, IOptions<DatabaseSettings> settings)
        {
            _restaurantCollection = database.GetCollection<Restaurant>(settings.Value.RestaurantCollectionName);
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await _restaurantCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(string id)
        {
            return await _restaurantCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantCollection.InsertOneAsync(restaurant);
        }

        public async Task<bool> UpdateRestaurantAsync(string id, Restaurant restaurant)
        {
            var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, id);
            var update = Builders<Restaurant>.Update
                .Set(r => r.Name, restaurant.Name)
                .Set(r => r.Address, restaurant.Address)
                .Set(r => r.Phone, restaurant.Phone)
                .Set(r => r.ImageUrl, restaurant.ImageUrl);

            var result = await _restaurantCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteRestaurantAsync(string id)
        {
            var result = await _restaurantCollection.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
