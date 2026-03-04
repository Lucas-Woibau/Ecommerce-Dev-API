using EcommerceDev.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceDev.Infrastructure.Persistence
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemReview> OrderProductReviews { get; set; }
        public DbSet<OrderUpdate> OrderUpdates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.Id);

                e.HasMany(c => c.Addresses)
                    .WithOne()
                    .HasForeignKey(c => c.IdCustomer)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.Orders)
                    .WithOne(o => o.Customer)
                    .HasForeignKey(o => o.IdCustomer)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.Reviews)
                    .WithOne(r => r.Customer)
                    .HasForeignKey(r => r.IdCustomer)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<CustomerAddress>(e =>
            {
                e.HasKey(c => c.Id);
            });

            builder.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);

                e.HasMany(o => o.Items)
                    .WithOne()
                    .HasForeignKey(o => o.IdOrder);

                e.HasMany(o => o.Updates)
                    .WithOne()
                    .HasForeignKey(o => o.IdOrder)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderItem>(e =>
            {
                e.HasKey(o => o.Id);

                e.HasOne(o => o.Review)
                    .WithOne(o => o.OrderItem)
                    .HasForeignKey<OrderItemReview>(o => o.IdOrderItem)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.Product)
                    .WithMany()
                    .HasForeignKey(o => o.IdProduct)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderItemReview>(e =>
            {
                e.HasKey(o => o.Id);
            });

            builder.Entity<OrderUpdate>(e =>
            {
                e.HasKey(o => o.Id);
            });

            builder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasMany(p => p.Images)
                    .WithOne()
                    .HasForeignKey(pi => pi.IdProduct)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProductCategory>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(c => c.IdCategory)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProductImage>(e =>
            {
                e.HasKey(p => p.Id);
            });

            base.OnModelCreating(builder);
        }
    }
}
