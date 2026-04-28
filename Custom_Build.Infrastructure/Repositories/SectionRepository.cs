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
    public class SectionRepository : ISectionRepository
    {
        public readonly ApplicationDbContext _dbContext;

        public SectionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> AddAsync(AddSectionDTO toAdd)
        {
            Section newSection = new Section()
            {
                Id = Guid.NewGuid(),
                Icon = toAdd.Icon,
                Name = toAdd.Name
            };

            _dbContext.Sections.Add(newSection);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> EditByIdAsync(EditSectionDTO newData)
        {
            Section? toEdit = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("section wasnt found" ,statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Icon = newData.Icon ?? toEdit.Icon;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<Section>> GetByIdAsync(Guid sectionId)
        {
            Section? section = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);

            if(section == null)
            {
                return Result<Section>.Failure("section wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<Section>.Success(section);
        }
        public async Task<Result> RemoveByIdAsync(Guid sectionId)
        {
            Section? toDel = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);

            if(toDel == null)
            {
                return Result.Failure("section wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Sections.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
