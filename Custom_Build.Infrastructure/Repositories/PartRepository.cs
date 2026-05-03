using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Result<Part>> AddAsync(AddPartDTO toAdd)
        {
            Part newPart = new Part()
            {
                Id = Guid.NewGuid(),
                Icon = toAdd.Icon,
                Name = toAdd.Name
            };

            _dbContext.Parts.Add(newPart);
            await _dbContext.SaveChangesAsync();

            return Result<Part>.Success(newPart);
        }
        public async Task<Result> EditByIdAsync(EditPartDTO newData)
        {
            Part? toEdit = await _dbContext.Parts.FirstOrDefaultAsync(p => p.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("part wasnt found" ,statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Icon = newData.Icon ?? toEdit.Icon;

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
    }
}
