using GameStore.Models.Entities;

namespace GameStore.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task DeleteOrderDetailAsync(int orderDetailId);
    }
}
