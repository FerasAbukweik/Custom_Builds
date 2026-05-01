using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Custom_Builds.Core.Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<CartItem> CartItems = new List<CartItem>();
        public List<Order> Orders = new List<Order>();
        public List<RefreshToken> refreshTokens = new List<RefreshToken>();
    }
}
