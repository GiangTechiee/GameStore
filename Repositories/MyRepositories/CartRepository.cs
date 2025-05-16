using GameStore.Data;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories.MyRepositories
{
    public class CartRepository : ICartRepository
    {
        private readonly GameStoreContext _context;

        public CartRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
            .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Game)
                       .ThenInclude(g => g.GameImages)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> GetCartByIdAsync(int cartId)
        {
            return await _context.Carts
            .Include(c => c.CartDetails)
            .ThenInclude(cd => cd.Game)
            .FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task<CartDetail> GetCartDetailByIdAsync(int cartDetailId)
        {
            return await _context.CartDetails
            .Include(cd => cd.Game)
            .FirstOrDefaultAsync(cd => cd.CartDetailId == cartDetailId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCartDetailAsync(CartDetail cartDetail)
        {
            await _context.CartDetails.AddAsync(cartDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartDetailAsync(CartDetail cartDetail)
        {
            _context.CartDetails.Update(cartDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartDetailAsync(int cartDetailId)
        {
            var cartDetail = await _context.CartDetails.FindAsync(cartDetailId);
            if (cartDetail != null)
            {
                _context.CartDetails.Remove(cartDetail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllCartDetailAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                _context.CartDetails.RemoveRange(cart.CartDetails);
                await _context.SaveChangesAsync();
            }
        }
    }
}
