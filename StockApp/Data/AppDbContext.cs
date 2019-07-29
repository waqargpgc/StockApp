using Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockApp.Data.Enitities;
using StockApp.Data.Enitities.EntityMappings;

namespace StockApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, UserClaim, UserRole, UserLogin, IdentityRoleClaim<string>, UserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
         
        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; } 
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryLocation> InventoryLocations { get; set; }
        //public DbSet<ProductInventory> ProductInventories { get; set; }
        public DbSet<TransactionHistory> Transactions { get; set; }
        public DbSet<ApplicationClaim> ApplicationClaims { get; set; }
          

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("AppUsers");
            builder.Entity<ApplicationRole>().ToTable("AppRoles");
            builder.Entity<UserRole>().ToTable("AppUserRole");
            builder.Entity<UserLogin>().ToTable("AppUserLogins");
            builder.Entity<UserClaim>().ToTable("AppUserClaims");
            builder.Entity<UserToken>().ToTable("AppUserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims"); 

            //builder.ApplyConfiguration(new ProductInventoryMap());
            builder.ApplyConfiguration(new InventoryMap());
            builder.ApplyConfiguration(new InventoryLocationMap());
            builder.ApplyConfiguration(new AreaMap());
        }

    }
}