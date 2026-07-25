using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Enums;
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

            
            // add roles

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole()
                {
                    Id = Guid.Parse("7c9e31d4-82f5-4e1b-a639-2d14e08f5193"),
                    Name = nameof(RolesEnum.Admin),
                    NormalizedName = nameof(RolesEnum.Admin).ToUpper(),
                    ConcurrencyStamp = "4b8f19e2-36c7-4d9a-8b15-20e8d3fa91a4"
                },
                new ApplicationRole()
                {
                    Id = Guid.Parse("a1d7f40e-5c82-411a-96e3-2b8f9e01d43c"),
                    Name = nameof(RolesEnum.User),
                    NormalizedName = nameof(RolesEnum.User).ToUpper(),
                    ConcurrencyStamp = "e82c19a4-67d1-4b3f-b982-14d20f5a89e6"
                }
            );
            
            // refresh token relations -----------------------------------------
            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.refreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<RefreshToken>()
                .HasIndex(rt => rt.RefreshTokenString)
                .IsUnique(true);

            
            
            // modification relations -------------------------------------------------
            builder.Entity<Modification>()
                .HasOne(m => m.Section)
                .WithMany(s => s.Modifications)
                .HasForeignKey(o => o.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

             
            
            // section relations -----------------------------------------------------
            builder.Entity<Section>()
                .HasOne(s => s.Part)
                .WithMany(p => p.Sections)
                .HasForeignKey(s => s.PartId)
                .OnDelete(DeleteBehavior.Cascade);

            
            
            // cart item relations -----------------------------------------------------
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

            
            
            // order relations --------------------------------------------------------
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            
            // order items relations ------------------------------------------------
            builder.Entity<OrderItem>()
                .HasOne(o => o.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<OrderItem>()
                .HasOne(o => o.CustomBuild)
                .WithOne(cb => cb.OrderItem)
                .HasForeignKey<OrderItem>(o => o.CustomBuildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderedItems)
                .HasForeignKey(oi => oi.OrderId);

            
            
            // custom build relations ---------------------------------------------
            builder.Entity<CustomBuild>()
                .HasMany(cb => cb.Modifications)
                .WithMany(m => m.CustomBuilds)
                .UsingEntity(e => e.ToTable("CustomBuilds_Modifications_ManyToMany"));

            builder.Entity<CustomBuild>()
                .HasOne(cb => cb.User)
                .WithMany(u => u.CustomBuilds)
                .HasForeignKey(cb => cb.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            
            
            // message relations --------------------------------------------------
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessageSenders)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Message>()
                .HasOne(m => m.ChatGroup)
                .WithMany(cg => cg.Messages)
                .HasForeignKey(m => m.ChatGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            

            // chat group relations ---------------------------------------------
            builder.Entity<ChatGroup>()
                .HasOne(cg => cg.User)
                .WithOne(u => u.ChatGroup)
                .HasForeignKey<ChatGroup>(cg => cg.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ChatGroup>()
                .HasMany(cg => cg.Supporters)
                .WithMany(u => u.ChatGroups)
                .UsingEntity(e => e.ToTable("ChatGroup_User_ManyToMany"));
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Modification> Modifications { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<CustomBuild> CustomBuilds { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<ChatGroup> ChatGroups { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
    }
}
