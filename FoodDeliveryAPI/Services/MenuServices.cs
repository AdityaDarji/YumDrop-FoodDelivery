using FoodDeliveryAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodDeliveryAPI.Services
{
    public class MenuServices
    {
        private readonly IMongoCollection<Menu> _menuCollection;

        public MenuServices(IMongoDatabase database, IOptions<DatabaseSettings> settings)
        {
            _menuCollection = database.GetCollection<Menu>(settings.Value.MenuCollectionName);
        }

        public async Task<List<Menu>> GetMenuItemsAsync()
        {
            return await _menuCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Menu> GetMenuItemByIdAsync(string id)
        {
            return await _menuCollection.Find(menu => menu.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMenuItemAsync(Menu menu)
        {
            await _menuCollection.InsertOneAsync(menu);
        }
        public async Task<List<Menu>> GetMenuByRestaurantAsync(string restaurantId)
        {
            return await _menuCollection.Find(menu => menu.RestaurantId == restaurantId).ToListAsync();
        }

        public async Task<bool> UpdateMenuItemAsync(string id, Menu menu)
        {
            var filter = Builders<Menu>.Filter.Eq(m => m.Id, id);
            var update = Builders<Menu>.Update
                .Set(m => m.Name, menu.Name)
                .Set(m => m.Description, menu.Description)
                .Set(m => m.Price, menu.Price)
                .Set(m => m.Category, menu.Category)
                .Set(m => m.ImageUrl, menu.ImageUrl);

            var result = await _menuCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteMenuItemAsync(string id)
        {
            var result = await _menuCollection.DeleteOneAsync(m => m.Id == id);
            return result.DeletedCount > 0;
        }

    }
}
