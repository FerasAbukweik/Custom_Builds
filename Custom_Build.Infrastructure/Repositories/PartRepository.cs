using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Part>> AddAsync(Part toAdd)
        {
            _dbContext.Parts.Add(toAdd);
            await _dbContext.SaveChangesAsync();

            return Result<Part>.Success(toAdd);
        }
        public async Task<Result> EditByIdAsync(EditPartDTO newData)
        {
            Part? toEdit = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("part wasnt found" ,statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Icon = newData.Icon ?? toEdit.Icon;
            toEdit.Name = newData.Name ?? toEdit.Name;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<Part>> GetByIdAsync(Guid partId)
        {
            Part? part = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == partId);

            if(part == null)
            {
                return Result<Part>.Failure("part wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<Part>.Success(part);
        }
        public async Task<Result> RemoveByIdAsync(Guid partId)
        {
            Part? toDel = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == partId);

            if(toDel == null)
            {
                return Result.Failure("part wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Parts.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<List<Part>>> FilterAsync(Expression<Func<Part, bool>> extraChecks, Expression<Func<Part, object>>[]? includes = null)
        {

            var partQuery = _dbContext.Parts.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    partQuery = partQuery.Include(include);
                }
            }

            List<Part> parts = await partQuery.Where(extraChecks).ToListAsync();

            return Result<List<Part>>.Success(parts);
        }
        public async Task<Result<List<Part>>> GetAllPartsIncludingAllData()
        {
            var allParts = await _dbContext.Parts.Include(p => p.Sections).ThenInclude(s => s.Modifications).ToListAsync();

            return Result<List<Part>>.Success(allParts);
        }
        public async Task<Result> LinkSectionAsync(Guid partId, Section section)
        {
            Part? targetPart = await _dbContext.Parts.Include(p => p.Sections).FirstOrDefaultAsync(p => p.Id == partId);
            if(targetPart == null)
            {
                return Result.Failure("Part Want found" , HttpStatusCode.NotFound);
            }

            targetPart.Sections.Add(section);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
