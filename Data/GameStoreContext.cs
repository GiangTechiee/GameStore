using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameStore.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình chuyển đổi DateTime sang UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        modelBuilder.Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        modelBuilder.Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(nullableDateTimeConverter);
                    }
                }
            }


            // 1. Mối quan hệ Game - GameImage (Một-nhiều)
            modelBuilder.Entity<Game>()
                .HasMany(g => g.GameImages)
                .WithOne(i => i.Game)
                .HasForeignKey(i => i.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Game sẽ xóa tất cả GameImage liên quan

            // 2. Mối quan hệ User - Cart (Một-một)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa User sẽ xóa Cart liên quan

            // 3. Mối quan hệ Cart - CartDetail (Một-nhiều)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartDetails)
                .WithOne(cd => cd.Cart)
                .HasForeignKey(cd => cd.CartId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Cart sẽ xóa tất cả CartDetail liên quan

            // 4. Mối quan hệ CartDetail - Game (Nhiều-một)
            modelBuilder.Entity<CartDetail>()
                .HasOne(cd => cd.Game)
                .WithMany()
                .HasForeignKey(cd => cd.GameId)
                .OnDelete(DeleteBehavior.Restrict); // Không thể xóa Game nếu có CartDetail liên quan

            // 5. Mối quan hệ User - Wishlist (Một-một)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wishlist)
                .WithOne(w => w.User)
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa User sẽ xóa Wishlist liên quan

            // 6. Mối quan hệ Wishlist - WishlistItem (Một-nhiều)
            modelBuilder.Entity<Wishlist>()
                .HasMany(w => w.WishlistItems)
                .WithOne(wi => wi.Wishlist)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Wishlist sẽ xóa tất cả WishlistItem liên quan

            // 7. Mối quan hệ WishlistItem - Game (Nhiều-một)
            modelBuilder.Entity<WishlistItem>()
                .HasOne(wi => wi.Game)
                .WithMany()
                .HasForeignKey(wi => wi.GameId)
                .OnDelete(DeleteBehavior.Restrict); // Không thể xóa Game nếu có WishlistItem liên quan

            // 8. Mối quan hệ User - Order (Một-nhiều)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa User sẽ xóa tất cả Order liên quan

            // 9. Mối quan hệ Order - OrderDetail (Một-nhiều)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Order sẽ xóa tất cả OrderDetail liên quan

            // 10. Mối quan hệ OrderDetail - Game (Nhiều-một)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Game)
                .WithMany()
                .HasForeignKey(od => od.GameId)
                .OnDelete(DeleteBehavior.Restrict); // Không thể xóa Game nếu có OrderDetail liên quan

            // 11. Mối quan hệ Game - Category (Nhiều-nhiều) thông qua GameCategory
            modelBuilder.Entity<GameCategory>()
                .HasKey(gc => new { gc.GameId, gc.CategoryId });

            modelBuilder.Entity<GameCategory>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.GameCategories)
                .HasForeignKey(gc => gc.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameCategory>()
                .HasOne(gc => gc.Category)
                .WithMany(c => c.GameCategories)
                .HasForeignKey(gc => gc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 12. Mối quan hệ User - Role (Nhiều-một)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict); // Không thể xóa Role nếu có User liên quan
        }
    }
}
