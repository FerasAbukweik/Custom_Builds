using Custom_Builds.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Core.Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();

        public List<CartItem> CartItems = [];
        public List<Order> Orders = [];
        public List<RefreshToken> refreshTokens = [];
        public List<Message> MessageReceivers = [];
        public List<Message> MessageSenders = [];
        public List<CustomBuild> CustomBuilds = [];
        public List<ChatGroup> ChatGroups = [];
        public ChatGroup? ChatGroup { get; set; }
    }
}
