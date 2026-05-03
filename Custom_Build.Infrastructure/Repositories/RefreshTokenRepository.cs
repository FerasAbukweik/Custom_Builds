using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenRepository(ApplicationDbContext dbcontext ,
            UserManager<ApplicationUser> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }


        public async Task<Result<RefreshToken>> AddAsync(AddRefreshTokenDTO tokenInfo)
        {
            var getRefreshTokenResult = await GetFromRefreshTokenStringAsync(tokenInfo.RefreshTokenString);
            if (getRefreshTokenResult.IsSuccess)
            {
                return Result<RefreshToken>.Failure("refresh token already exists");
            }

            RefreshToken toAdd = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                ExpierDate = tokenInfo.ExpierDate,
                RefreshTokenString = tokenInfo.RefreshTokenString,
                UserId = tokenInfo.UserId,
            };

            _dbcontext.RefreshTokens.Add(toAdd);
            await _dbcontext.SaveChangesAsync();

            return Result<RefreshToken>.Success(toAdd);
        }
        public async Task<Result<RefreshToken>> GetFromRefreshTokenStringAsync(string refreshToken)
        {
            RefreshToken? refToken = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenString == refreshToken);

            if(refToken == null)
            {
                return Result<RefreshToken>.Failure("refresh token wanst found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<RefreshToken>.Success(refToken);
        }
        public async Task<Result<ApplicationUser>> GetUserFromRefreshTokenStringAsync(string refreshTokenString)
        {
            Result<RefreshToken> refreshTokenResult = await GetFromRefreshTokenStringAsync(refreshTokenString);

            if(!refreshTokenResult.IsSuccess)
            {
                return Result<ApplicationUser>.Failure(refreshTokenResult.ErrorMessage ?? "refresh token wasnt found", refreshTokenResult.StatusCode);
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(refreshTokenResult.Value!.UserId.ToString());

            if(user == null)
            {
                return Result<ApplicationUser>.Failure("user wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<ApplicationUser>.Success(user);
        }
        public async Task<Result<RefreshToken>> GetFromIdAsync(Guid refreshTokenId)
        {
            RefreshToken? refToken = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId);

            if(refToken == null)
            {
                return Result<RefreshToken>.Failure("refresh token wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<RefreshToken>.Success(refToken);
        }
        public async Task<Result> RemoveByIdAsync(Guid tokenId)
        {
            Result<RefreshToken> refreshTokenResult = await GetFromIdAsync(tokenId);

            if(!refreshTokenResult.IsSuccess)
            {
                return Result.Failure(refreshTokenResult.ErrorMessage ?? "refresh token wasnt found", refreshTokenResult.StatusCode);
            }

            _dbcontext.RefreshTokens.Remove(refreshTokenResult.Value!);
            await _dbcontext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> RemoveByRefreshTokenStringAsync(string refreshToken)
        {
            RefreshToken? toDel = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt =>
            rt.RefreshTokenString == refreshToken);

            if(toDel == null)
            {
                return Result.Failure("refresh token wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            _dbcontext.RefreshTokens.Remove(toDel);
            await _dbcontext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
