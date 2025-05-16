namespace GameStore.Models.DTOs.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartDetailDto> CartDetails { get; set; } = new List<CartDetailDto>();
    }
}
