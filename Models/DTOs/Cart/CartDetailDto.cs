namespace GameStore.Models.DTOs.Cart
{
    public class CartDetailDto
    {
        public int CartDetailId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }
    }
}
