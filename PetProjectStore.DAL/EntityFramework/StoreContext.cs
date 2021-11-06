using Microsoft.EntityFrameworkCore;
using PetProjectStore.DAL.Entities;

namespace PetProjectStore.DAL.EntityFramework
{
    public class StoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(h => h.Products)
                .WithMany(w => w.Orders);
        }
    }
}
