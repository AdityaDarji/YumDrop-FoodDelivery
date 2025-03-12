using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FoodDeliveryAPI.Models
{
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("restaurantId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RestaurantId { get; set; } // Link menu to a restaurant

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
