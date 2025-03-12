namespace FoodDeliveryAPI.Models
{
    public class DatabaseSettings
    {
        public required string DatabaseName { get; set; }
        public required string OrderCollectionName { get; set; }
        public required string UserCollectionName { get; set; }
        public required string RestaurantCollectionName { get; set; }
        public required string MenuCollectionName { get; set; }
    }
}
