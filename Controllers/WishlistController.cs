using GameStore.Models.DTOs.Wishlist;
using GameStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
                if (wishlist == null)
                {
                    return NotFound(new { Message = "Wishlist not found" });
                }
                return Ok(wishlist);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the wishlist" });
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddWishlistItem([FromBody] WishlistItemCreateDto createDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var wishlistItem = await _wishlistService.AddWishlistItemAsync(userId, createDto);
                return CreatedAtAction(nameof(GetWishlist), new { }, wishlistItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the wishlist item" });
            }
        }

        [HttpDelete("items/{wishlistItemId}")]
        public async Task<IActionResult> RemoveWishlistItem(int wishlistItemId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _wishlistService.RemoveWishlistItemAsync(userId, wishlistItemId);
                if (!result)
                {
                    return NotFound(new { Message = "Wishlist item not found or does not belong to the user" });
                }
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An error occurred while removing the wishlist item" });
            }
        }
    }
}
