using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Custom_Builds.Infrastructure.Repositories
{
    internal class FieldRepository : IFieldRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FieldRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<Result> AddAsync(AddFieldDTO toAdd)
        {
            Field newField = new Field()
            {
                Id = Guid.NewGuid(),
                SectionId = toAdd.SectionId,
                Title = toAdd.Title,
                Type = toAdd.Type,
            };

            _dbContext.Fields.Add(newField);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> EditByIdAsync(EditFieldDTO newData)
        {
            Field? toEdit = await _dbContext.Fields.FirstOrDefaultAsync(f => f.Id == newData.Id);

            if (toEdit == null)
            {
                return Result.Failure("field wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Title = newData.Title ?? toEdit.Title;
            toEdit.SectionId = newData.SectionId ?? toEdit.SectionId;
            toEdit.Type = newData.Type ?? toEdit.Type;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<Field>> GetByIdAsync(Guid fieldId)
        {
            Field? field = await _dbContext.Fields.FirstOrDefaultAsync(f => f.Id == fieldId);

            if(field == null)
            {
                return Result<Field>.Failure("field wasnt found" , statusCode: HttpStatusCode.NotFound);
            }


            return Result<Field>.Success(field);
        }
        public async Task<Result> RemoveByIdAsync(Guid fieldId)
        {
            Field? toDel = await _dbContext.Fields.FirstOrDefaultAsync(f => f.Id == fieldId);

            if (toDel == null)
            {
                return Result.Failure("field want found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Fields.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
