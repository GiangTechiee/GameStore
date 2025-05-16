using GameStore.Models.Entities;

namespace GameStore.Repositories.IRepositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<CartDetail> GetCartDetailByIdAsync(int cartDetailId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(int cartId);
        Task AddCartDetailAsync(CartDetail cartDetail);
        Task UpdateCartDetailAsync(CartDetail cartDetail);
        Task DeleteCartDetailAsync(int cartDetailId);

        Task DeleteAllCartDetailAsync(int userId);
    }
}
