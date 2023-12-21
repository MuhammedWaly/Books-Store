using LoginAndRegister.Models;
using LoginAndRegister.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginAndRegister.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUsers>().ToTable("Users","security");
            
            builder.Entity<IdentityRole>().ToTable("Roles", "security");

            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");

            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");

            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");

            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            


        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDatail> CartDatails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
       
    }
}