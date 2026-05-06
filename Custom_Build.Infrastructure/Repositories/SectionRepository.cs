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
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SectionRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<Result<Section>> AddAsync(Section toAdd)
        {
            _dbContext.Sections.Add(toAdd);
            await _dbContext.SaveChangesAsync();

            return Result<Section>.Success(toAdd);
        }
        public async Task<Result> EditByIdAsync(EditSectionDTO newData)
        {
            Section? toEdit = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("section wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Title = newData.Title ?? toEdit.Title;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<Section>> GetByIdAsync(Guid sectionId)
        {
            Section? section = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);

            if(section == null)
            {
                return Result<Section>.Failure("section wasnt found" , statusCode: HttpStatusCode.NotFound);
            }


            return Result<Section>.Success(section);
        }
        public async Task<Result> RemoveByIdAsync(Guid sectionId)
        {
            Section? toDel = await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);

            if (toDel == null)
            {
                return Result.Failure("section want found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Sections.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<List<Section>>> FilterAsync(Expression<Func<Section, bool>> extraChecks, Expression<Func<Section, object>>[]? includes = null)
        {

            var sectionQuery = _dbContext.Sections.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    sectionQuery = sectionQuery.Include(include);
                }
            }

            List<Section> sections = await sectionQuery.Where(extraChecks).ToListAsync();

            return Result<List<Section>>.Success(sections);
        }
        public async Task<Result> AddModificationAsync(Guid sectionId , Modification modification)
        {
            Section? targetSection = await _dbContext.Sections.Include(s => s.Modifications).FirstOrDefaultAsync(s => s.Id == sectionId);
            if(targetSection == null)
            {
                return Result.Failure("Section not found" , HttpStatusCode.NotFound);
            }

            targetSection.Modifications.Add(modification);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
