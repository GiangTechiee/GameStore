namespace GameStore.Models.DTOs.Order
{
    public class CreateOrderDetailDto
    {
        public int GameId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
