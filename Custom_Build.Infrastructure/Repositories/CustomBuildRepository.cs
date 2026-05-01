using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CustomBuildRepository : ICustomBuildRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomBuildRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<Result<Guid>> AddAsync(AddCustomBuildDTO toAdd)
        {
            CustomBuild newCB = new CustomBuild
            {
                Id = Guid.NewGuid(),
                CustomBuildType = toAdd.CustomBuildType
            };

            _dbContext.CustomBuilds.Add(newCB);
            await _dbContext.SaveChangesAsync();

            return Result<Guid>.Success(newCB.Id);
        }

        public async Task<Result<Guid>> AddEntityAsync(CustomBuild customBuild)
        {
            _dbContext.CustomBuilds.Add(customBuild);
            await _dbContext.SaveChangesAsync();
            return Result<Guid>.Success(customBuild.Id);
        }

        public async Task<Result> EditByIdAsync(EditCustomBuildDTO newData)
        {
            CustomBuild? toEdit = await _dbContext.CustomBuilds.FirstOrDefaultAsync(c => c.Id == newData.Id);
            if (toEdit == null)
            {
                return Result.Failure("custom build wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            toEdit.CustomBuildType = newData.CustomBuildType ?? toEdit.CustomBuildType;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId)
        {
            CustomBuild? customBuild = await _dbContext.CustomBuilds.FirstOrDefaultAsync(c => c.Id == customBuildId);
            if (customBuild == null)
            {
                return Result<CustomBuild>.Failure("custom build wasnt found", statusCode: HttpStatusCode.NotFound);
            }
            
            return Result<CustomBuild>.Success(customBuild);
        }

        public async Task<Result> RemoveByIdAsync(Guid customBuildId)
        {
            CustomBuild? toDel = await _dbContext.CustomBuilds.FirstOrDefaultAsync(c => c.Id == customBuildId);
            if (toDel == null)
            {
                return Result.Failure("custom build wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.CustomBuilds.Remove(toDel);
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
    }
}
