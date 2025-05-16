namespace GameStore.Models.DTOs.Order
{
    public class CreateOrderDto
    {
        public int? UserId { get; set; } 
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
