using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class RefreshTokenRepositry : IRefreshTokenRepositry
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenRepositry(ApplicationDbContext dbcontext ,
            UserManager<ApplicationUser> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public async Task AddRefrehTokenAsync(AddRefreshTokenDTO tokenInfo)
        {
            if(GetRefreshTokenFromRefreshTokenStringAsync(tokenInfo.RefreshTokenString) != null)
            {
                throw new Exception("Refresh Token ALready Exirsts");
            }

            RefreshToken toAdd = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                ExpierDate = tokenInfo.ExpierDate,
                RefreshTokenString = tokenInfo.RefreshTokenString,
                UserId = Guid.NewGuid(),
            };

            _dbcontext.RefreshTokens.Add(toAdd);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<RefreshToken?> GetRefreshTokenFromRefreshTokenStringAsync(string refreshToken)
        {
            RefreshToken? refToken = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenString == refreshToken);

            return refToken;
        }
        public async Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(Guid userId)
        {
            List<RefreshToken> tokens = await _dbcontext.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();

            return tokens;
        }
        public async Task<ApplicationUser?> GetUserFromRefreshTokenStringAsync(string refreshTokenString)
        {
            RefreshToken? refreshToken = await GetRefreshTokenFromRefreshTokenStringAsync(refreshTokenString);

            if(refreshToken == null)
            {
                return null;
            }

            ApplicationUser? user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());

            return user;
        }
        public async Task<RefreshToken?> GetRefreshTokenFromRefreshTokenIdAsync(Guid refreshTokenId)
        {
            RefreshToken? refToken = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId);

            return refToken;
        }
        public async Task<bool> RemoveRefreshTokenByIdAsync(Guid tokenId)
        {
            RefreshToken? refreshToken = await GetRefreshTokenFromRefreshTokenIdAsync(tokenId);

            if(refreshToken == null)
            {
                return false;
            }

            _dbcontext.RefreshTokens.Remove(refreshToken);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveRefreshTokenByRefreshTokenStringAsync(string refreshToken)
        {
            RefreshToken? toDel = await _dbcontext.RefreshTokens.FirstOrDefaultAsync(rt =>
            rt.RefreshTokenString == refreshToken);

            if(toDel == null)
            {
                return false;
            }

            _dbcontext.RefreshTokens.Remove(toDel);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveUserRefreshTokensAsync(Guid UserId)
        {
            List<RefreshToken> toDel = await _dbcontext.RefreshTokens.Where(rt => rt.UserId == UserId).ToListAsync();

            if(toDel.Count() <= 0)
            {
                return false;
            }

            _dbcontext.RemoveRange(toDel);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}
