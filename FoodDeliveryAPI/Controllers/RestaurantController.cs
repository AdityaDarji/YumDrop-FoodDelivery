using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantServices _restaurantServices;

        public RestaurantController(RestaurantServices restaurantServices)
        {
            _restaurantServices = restaurantServices;
        }

        [HttpGet("get-restaurants")]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _restaurantServices.GetRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById(string id)
        {
            var restaurant = await _restaurantServices.GetRestaurantByIdAsync(id);
            return restaurant == null ? NotFound() : Ok(restaurant);
        }

        [HttpPost("add-restaurant")]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            await _restaurantServices.CreateRestaurantAsync(restaurant);
            return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
        }

        [HttpPut("update-restaurant/{id}")]
        public async Task<IActionResult> UpdateRestaurant(string id, [FromBody] Restaurant restaurant)
        {
            var updated = await _restaurantServices.UpdateRestaurantAsync(id, restaurant);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete-restaurant/{id}")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            var deleted = await _restaurantServices.DeleteRestaurantAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
