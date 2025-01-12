using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackImage> FeedbackImages { get; set; }
        public DbSet<ProductVideo> ProductVideos { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<RelatedCategory> RelatedCategories { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<PriceHistory> PricesHistory { get; set; }
        public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CategoryCharacteristic>()
                .HasKey(cc => new { cc.CategoryId, cc.CharacteristicId });

            modelBuilder.Entity<CategoryCharacteristic>()
                .HasOne(cc => cc.Category)
                .WithMany(c => c.CategoryCharacteristics)
                .HasForeignKey(cc => cc.CategoryId);

            modelBuilder.Entity<CategoryCharacteristic>()
                .HasOne(cc => cc.Characteristic)
                .WithMany(ch => ch.CategoryCharacteristics)
                .HasForeignKey(cc => cc.CharacteristicId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProductId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FeedbackImage>()
                .HasOne(fi => fi.Feedback)
                .WithMany(f => f.FeedbackImages)
                .HasForeignKey(fi => fi.FeedbackId);

            modelBuilder.Entity<ProductVideo>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.ProductVideos)
                .HasForeignKey(pv => pv.ProductId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Instruction>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Instructions)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<RelatedCategory>()
                .HasKey(rc => new { rc.CategoryId, rc.RelatedCategoryId });

            modelBuilder.Entity<RelatedCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.RelatedCategories)
                .HasForeignKey(rc => rc.CategoryId);

            modelBuilder.Entity<RelatedCategory>()
                .HasOne(rc => rc.RelatedCategoryItem)
                .WithMany()
                .HasForeignKey(rc => rc.RelatedCategoryId);

            modelBuilder.Entity<Price>()
                .HasKey(p => new { p.ProductId, p.SellerId });

            modelBuilder.Entity<Price>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<Price>()
                .HasOne(pr => pr.Seller)
                .WithMany(s => s.Prices)
                .HasForeignKey(pr => pr.SellerId);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(ph => ph.Product)
                .WithMany()
                .HasForeignKey(ph => ph.ProductId);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(ph => ph.Seller)
                .WithMany()
                .HasForeignKey(ph => ph.SellerId);

            modelBuilder.Entity<ProductCharacteristic>()
                .HasKey(pc => new { pc.ProductId, pc.CharacteristicId });

            modelBuilder.Entity<ProductCharacteristic>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCharacteristics)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCharacteristic>()
                .HasOne(pc => pc.Characteristic)
                .WithMany(ch => ch.ProductCharacteristics)
                .HasForeignKey(pc => pc.CharacteristicId);

            modelBuilder.Entity<Seller>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sellers)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }

    }
}
