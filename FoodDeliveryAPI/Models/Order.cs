using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FoodDeliveryAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string CustomerName { get; set; } = string.Empty;
        public string FoodItem { get; set; } = string.Empty;
        public double Price { get; set; }
    }
}
