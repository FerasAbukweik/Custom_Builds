using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IRefreshTokenServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Custom_Builds.Core.Services.RefreshTokenServices
{
    public class GenerateRefreshTokenService : IGenerateRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepositry;
        private readonly IConfiguration _configuration;

        public GenerateRefreshTokenService(IRefreshTokenRepository refreshTokenRepositry,
                                           IConfiguration configuration)
        {
            _refreshTokenRepositry = refreshTokenRepositry;
            _configuration = configuration;
        }


        public async Task<Result<RefreshTokenDTO>> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            byte[] bytes = new byte[64];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            string refToken = Convert.ToBase64String(bytes);

            // store refresh token in the DB
            var addResult = await _refreshTokenRepositry.AddAsync(new RefreshToken()
            {
                Id = Guid.NewGuid(),
                ExpierDate = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:RefreshTokenLife"]!)),
                RefreshTokenString = refToken,
                UserId = user.Id,   
            });

            if (!addResult.IsSuccess) return addResult.MapFailure<RefreshTokenDTO>();

            return Result<RefreshTokenDTO>.Success(addResult.Value!.toDTO());
        }
    }
}
