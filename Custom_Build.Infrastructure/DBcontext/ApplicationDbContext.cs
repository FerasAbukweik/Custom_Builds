using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.TokenEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Custom_Builds.Infrastructure.DBcontext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.refreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Modification>()
                .HasOne(m => m.Section)
                .WithMany(s => s.Modifications)
                .HasForeignKey(m => m.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Section>()
                .HasOne(s => s.Part)
                .WithMany(p => p.Sections)
                .HasForeignKey(s => s.PartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<CartItem>()
               .HasOne(ci => ci.CustomBuild)
               .WithMany(cb => cb.CartItems)
               .HasForeignKey(ci => ci.CustomBuildId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Order>()
                .HasOne(o => o.CustomBuild)
                .WithMany(cb => cb.orders)
                .HasForeignKey(o => o.CustomBuildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomBuild>()
                .HasMany(cb => cb.Modifications)
                .WithMany(m => m.CustomBuilds)
                .UsingEntity(j => j.ToTable("CustomBuildModifications"));

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomBuild>()
                .HasOne(cb => cb.Creator)
                .WithMany(u => u.CustomBuilds)
                .HasForeignKey(cb => cb.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Modification> Modifications { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<CartItem> Cart { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<CustomBuild> CustomBuilds { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
