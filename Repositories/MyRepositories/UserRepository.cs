using GameStore.Data;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories.MyRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GameStoreContext _context;

        public UserRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .OrderBy(u => u.UserId)
                    .ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Orders)
                .Include(u => u.Cart)
                .Include(u => u.Wishlist)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return;

            if (user.Orders?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete user with existing orders.");
            }

            if (user.Cart != null)
            {
                _context.Carts.Remove(user.Cart);
            }

            if (user.Wishlist != null)
            {
                _context.Wishlists.Remove(user.Wishlist);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
