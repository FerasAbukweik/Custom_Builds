using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Net;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class ModificationsRepository : IModificationsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ModificationsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Result<Modification>> GetFromIdAsync(Guid modificationId, Expression<Func<Modification, object>>[]? includes)
        {
            var modificationQuery = _dbContext.Modifications.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    modificationQuery.Include(include);
                }
            }

            Modification? modification = await modificationQuery.FirstOrDefaultAsync(m => m.Id == modificationId);

            if (modification == null)
            {
                return Result<Modification>.Failure("modification wasnt found", statusCode: HttpStatusCode.NotFound);
            }

            return Result<Modification>.Success(modification);
        }
        public async Task<Result<Modification>> AddAsync(Modification toAdd)
        {
            Modification newModification = new Modification()
            {
                Id = Guid.NewGuid(),
                Description = toAdd.Description,
                Icon = toAdd.Icon,
                Name = toAdd.Name,
                Price = toAdd.Price,
                Type = toAdd.Type,
                Value = toAdd.Value,
            };

            _dbContext.Modifications.Add(newModification);
            await _dbContext.SaveChangesAsync();

            return Result<Modification>.Success(newModification);
        }
        public async Task<Result> EditByIdAsync(EditModificationDTO newData)
        {
            Modification? toEdit = await _dbContext.Modifications.FirstOrDefaultAsync(m => m.Id == newData.Id);

            if(toEdit == null)
            {
                return Result.Failure("item wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Price = newData.Price ?? toEdit.Price;
            toEdit.Icon = newData.Icon ?? toEdit.Icon;
            toEdit.Description = newData.Description ?? toEdit.Description;
            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Type = newData.Type ?? toEdit.Type;
            toEdit.Value = newData.Value ?? toEdit.Value;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> RemoveByIdAsync(Guid modificationId)
        {
            Modification? toDel = await _dbContext.Modifications.FirstOrDefaultAsync(m => m.Id == modificationId);

            if (toDel == null)
            {
                return Result.Failure("item wasnt Found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Modifications.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<List<Modification>>> FilterAsync(Expression<Func<Modification, bool>> extraChecks)
        {

            var modifications = await _dbContext.Modifications.Where(extraChecks).ToListAsync();

            return Result<List<Modification>>.Success(modifications);
        }

    }
}
