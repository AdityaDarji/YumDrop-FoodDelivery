using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _orderServices;

        public OrderController(OrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderServices.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            return order == null ? NotFound(new { message = "Order not found" }) : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderServices.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] Order order)
        {
            var updated = await _orderServices.UpdateOrderAsync(id, order);
            return updated ? NoContent() : NotFound(new { message = "Order not found" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id) 
        {
            var deleted = await _orderServices.DeleteOrderAsync(id); 
            return deleted ? NoContent() : NotFound(new { message = "Order not found" });
        }
    }
}
