using GameStore.Models.DTOs.Order;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderController(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        // GET: api/Order/{orderId}
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var orderResponse = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    GameId = od.GameId,
                    GameName = od.Game?.Title ?? "Unknown",
                    UnitPrice = od.UnitPrice,
                    ImageUrl = od.Game?.GameImages?.FirstOrDefault()?.ImageUrl ?? ""
                }).ToList()
            };

            return Ok(orderResponse);
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderResponses = orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    GameId = od.GameId,
                    GameName = od.Game?.Title ?? "Unknown",
                    UnitPrice = od.UnitPrice,
                    ImageUrl = od.Game?.GameImages?.FirstOrDefault()?.ImageUrl ?? ""
                }).ToList()
            }).ToList();

            return Ok(orderResponses);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            }

            // Tính tổng tiền
            var totalAmount = request.OrderDetails.Sum(od => od.UnitPrice);

            // Tạo Order
            var order = new Order
            {
                UserId = request.UserId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDetails = request.OrderDetails.Select(od => new OrderDetail
                {
                    GameId = od.GameId,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            // Lưu Order vào cơ sở dữ liệu
            await _orderRepository.CreateOrderAsync(order);

            // Xóa giỏ hàng nếu người dùng đã đăng nhập
            if (request.UserId.HasValue)
            {
                var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId.Value);
                if (cart != null)
                {
                    await _cartRepository.DeleteCartAsync(cart.CartId);
                }
            }

            // Trả về phản hồi
            var orderResponse = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    GameId = od.GameId,
                    GameName = od.Game?.Title ?? "Unknown",
                    UnitPrice = od.UnitPrice,
                    ImageUrl = od.Game?.GameImages?.FirstOrDefault()?.ImageUrl ?? ""
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, orderResponse);
        }

        // PUT: api/Order/{orderId}
        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = request.Status;
            await _orderRepository.UpdateOrderAsync(order);

            return Ok("Order updated.");
        }

        // DELETE: api/Order/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            await _orderRepository.DeleteOrderAsync(orderId);
            return Ok("Order deleted.");
        }
    }
}
