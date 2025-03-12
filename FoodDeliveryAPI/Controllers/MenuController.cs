using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuServices _menuServices;

        public MenuController(MenuServices menuServices)
        {
            _menuServices = menuServices;
        }

        [HttpGet("get-menu-items")]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _menuServices.GetMenuItemsAsync();
            return Ok(menuItems);
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetMenuByRestaurant(string restaurantId)
        {
            var menuItems = await _menuServices.GetMenuByRestaurantAsync(restaurantId);
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(string id)
        {
            var menuItem = await _menuServices.GetMenuItemByIdAsync(id);
            return menuItem == null ? NotFound() : Ok(menuItem);
        }

        [HttpPost("add-menu-item")]
        public async Task<IActionResult> CreateMenuItem([FromBody] Menu menu)
        {
            await _menuServices.CreateMenuItemAsync(menu);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = menu.Id }, menu);
        }

        [HttpPut("update-menu-item/{id}")]
        public async Task<IActionResult> UpdateMenuItem(string id, [FromBody] Menu menu)
        {
            var updated = await _menuServices.UpdateMenuItemAsync(id, menu);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete-menu-item/{id}")]
        public async Task<IActionResult> DeleteMenuItem(string id)
        {
            var deleted = await _menuServices.DeleteMenuItemAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
