using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Core.Domain.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
    }
}
