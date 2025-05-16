namespace GameStore.Models.DTOs.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
