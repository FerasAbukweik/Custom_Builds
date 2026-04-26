using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.TokenEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;


namespace Custom_Builds.Infrastructure.DBcontext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
