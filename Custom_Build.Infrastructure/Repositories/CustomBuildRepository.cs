using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class CustomBuildRepository : ICustomBuildRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomBuildRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;


        public async Task<Result<CustomBuild>> AddAsync(AddCustomBuildDTOToDB toAdd)
        {
            CustomBuild newCB = new CustomBuild
            {
                Id = Guid.NewGuid(),
                CustomBuildType = toAdd.CustomBuildType,
                Modifications = toAdd.Modifications,
                CreatorId = toAdd.CreatorId
            };

            _dbContext.CustomBuilds.Add(newCB);
            await _dbContext.SaveChangesAsync();

            return Result<CustomBuild>.Success(newCB);
        }
        public async Task<Result<CustomBuild>> AddEntityAsync(CustomBuild toAdd)
        {
            _dbContext.CustomBuilds.Add(toAdd);
            await _dbContext.SaveChangesAsync();

            return Result<CustomBuild>.Success(toAdd);
        }
        public async Task<Result> EditByIdAsync(EditCustomBuildDTO newData)
        {
            CustomBuild? toEdit = await _dbContext.CustomBuilds.FirstOrDefaultAsync(c => c.Id == newData.Id);
            if (toEdit == null)
            {
                return Result.Failure("custom build wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            toEdit.CustomBuildType = newData.NewCustomBuildType ?? toEdit.CustomBuildType;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<CustomBuild>> GetByIdAsync(Guid customBuildId)
        {
            CustomBuild? customBuild = await _dbContext.CustomBuilds
                .Include(c => c.Modifications)
                .FirstOrDefaultAsync(c => c.Id == customBuildId);
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
        public async Task<Result<List<CustomBuild>>> FilterAsync(Expression<Func<CustomBuild, bool>> extraChecks, Expression<Func<CustomBuild, object>>[]? includes = null)
        {

            var CustomBuildQuery = _dbContext.CustomBuilds.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    CustomBuildQuery = CustomBuildQuery.Include(include);
                }
            }

            List<CustomBuild> customBuilds = await CustomBuildQuery.Where(extraChecks).ToListAsync();

            return Result<List<CustomBuild>>.Success(customBuilds);
        }

    }
}
