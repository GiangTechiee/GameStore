using GameStore.Models.DTOs.Cart;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/Cart/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDto>> GetCartByUserId(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                // Tạo giỏ hàng mới nếu chưa có
                cart = new Cart
                {
                    UserId = userId,
                    CartDetails = new List<CartDetail>()
                };
                await _cartRepository.AddCartAsync(cart);
            }

            var cartResponse = new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CartDetails = cart.CartDetails.Select(cd => new CartDetailDto
                {
                    CartDetailId = cd.CartDetailId,
                    GameId = cd.GameId,
                    GameName = cd.Game?.Title ?? "Unknown",
                    UnitPrice = cd.UnitPrice,
                    ImageUrl = cd.Game?.GameImages?.FirstOrDefault()?.ImageUrl ?? ""
                }).ToList()
            };

            return Ok(cartResponse);
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<ActionResult> AddToCart([FromBody] AddToCartDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            }

            // Kiểm tra xem giỏ hàng đã tồn tại chưa
            var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                // Tạo giỏ hàng mới nếu chưa có
                cart = new Cart
                {
                    UserId = request.UserId,
                    CartDetails = new List<CartDetail>()
                };
                await _cartRepository.AddCartAsync(cart);
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingDetail = cart.CartDetails?.FirstOrDefault(cd => cd.GameId == request.GameId);
            if (existingDetail != null)
            {
                // Trả về lỗi nếu sản phẩm đã tồn tại
                return BadRequest(new { message = "Product already exists in cart." });
            }

            // Thêm sản phẩm mới vào giỏ hàng
            var cartDetail = new CartDetail
            {
                CartId = cart.CartId,
                GameId = request.GameId,
                UnitPrice = request.UnitPrice
            };
            await _cartRepository.AddCartDetailAsync(cartDetail);

            return Ok(new { message = "Product added to cart.", cartId = cart.CartId });
        }

        // PUT: api/Cart/update-detail
        [HttpPut("update-detail")]
        public async Task<ActionResult> UpdateCartDetail([FromBody] UpdateCartDetailDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartDetail = await _cartRepository.GetCartDetailByIdAsync(request.CartDetailId);
            if (cartDetail == null)
            {
                return NotFound("Cart detail not found.");
            }

            cartDetail.UnitPrice = request.UnitPrice;
            await _cartRepository.UpdateCartDetailAsync(cartDetail);

            return Ok("Cart detail updated.");
        }

        // DELETE: api/Cart/remove-detail/{cartDetailId}
        [HttpDelete("remove-detail/{cartDetailId}")]
        public async Task<ActionResult> RemoveCartDetail(int cartDetailId)
        {
            var cartDetail = await _cartRepository.GetCartDetailByIdAsync(cartDetailId);
            if (cartDetail == null)
            {
                return NotFound("Cart detail not found.");
            }

            await _cartRepository.DeleteCartDetailAsync(cartDetailId);
            return Ok("Product removed from cart.");
        }

        // DELETE: api/Cart/{cartId}
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            await _cartRepository.DeleteCartAsync(cartId);
            return Ok("Cart deleted.");
        }

        [HttpDelete("user/{userId}")]
        public async Task<ActionResult> DeleteAllCartDetail(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            await _cartRepository.DeleteAllCartDetailAsync(userId);
            return Ok("All products removed from cart.");
        }
    }
}
