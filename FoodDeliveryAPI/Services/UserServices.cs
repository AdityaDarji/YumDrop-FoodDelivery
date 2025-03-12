using FoodDeliveryAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FoodDeliveryAPI.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserServices(IMongoDatabase database, IOptions<DatabaseSettings> settings)
        {
            _userCollection = database.GetCollection<User>(settings.Value.UserCollectionName);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateUser(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            

            var newUser = new User
            {
                Username = user.Username,  //Ensure username is assigned
                Email = user.Email,
                Phone = user.Phone,
                PasswordHash = user.PasswordHash,
                Address = user.Address
            };
            await _userCollection.InsertOneAsync(user);
        }
        public async Task<bool> UpdateUser(string userId, User updatedUser)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update
                .Set(u => u.Username, updatedUser.Username)
                .Set(u => u.Phone, updatedUser.Phone)
                .Set(u => u.Address, updatedUser.Address);

            var result = await _userCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public User? AuthenticateUser(string email, string password)
        {
            var user = _userCollection.Find(u => u.Email == email).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }
            return user; // Return user if credentials match
        }
    }
}
