namespace GameStore.Models.DTOs.Order
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
