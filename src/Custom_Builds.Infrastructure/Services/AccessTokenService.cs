using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Custom_Builds.Infrastructure.Services;

public class AccessTokenService(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration
    ) : IAccessTokenService
{
    public async Task<Result<string>> GenerateAccessTokenAsync(ApplicationUser user)
    {
        // claims
        List<Claim> claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),

            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        // addRoles
        var roles = await userManager.GetRolesAsync(user);
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("JWT:AccessTokenLife")),
                signingCredentials: creds
            );

        return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
    }
}