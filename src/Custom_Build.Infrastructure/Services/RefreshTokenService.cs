using System.Security.Cryptography;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Tokens;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class RefreshTokenService(
    IRefreshTokenRepository refreshTokenRepository,
    IConfiguration configuration,
    ILogger<RefreshTokenService> logger) : IRefreshTokenService
{
    public async Task<Result<RefreshTokenDTO>> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        byte[] bytes = new byte[64];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        string refToken = Convert.ToBase64String(bytes);

        // store refresh token in the DB
        var toAdd = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            ExpiryDate = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JWT:RefreshTokenLife"]!)),
            RefreshTokenString = refToken,
            UserId = userId,
        };
        refreshTokenRepository.Add(toAdd);

        if (!await refreshTokenRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogWarning("{serviceName}.{methodName} failed saving add refresh token",
                nameof(RefreshTokenService), nameof(GenerateRefreshTokenAsync));
            return Result<RefreshTokenDTO>.Failure("failed saving changes to DB");
        }

        return Result<RefreshTokenDTO>.Success(toAdd.toDTO());
    }
}