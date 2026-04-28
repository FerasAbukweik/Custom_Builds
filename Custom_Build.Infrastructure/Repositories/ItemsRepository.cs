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
    public class ItemsRepository : IItemsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> AddAsync(AddItemDTO toAdd)
        {
            Item newItem = new Item()
            {
                Id = Guid.NewGuid(),
                Description = toAdd.Description,
                FieldId = toAdd.FieldId,
                Icon = toAdd.Icon,
                Name = toAdd.Name,
                Price = toAdd.Price,
                Value = toAdd.Value,
            };

            _dbContext.Items.Add(newItem);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result> EditByIdAsync(EditItemDTO newData)
        {
            Item? toEdit = await _dbContext.Items.FirstOrDefaultAsync(it => it.Id == newData.Id);

            if(toEdit == null)
            {
                return Result.Failure("item wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            toEdit.Price = newData.Price ?? toEdit.Price;
            toEdit.Icon = newData.Icon ?? toEdit.Icon;
            toEdit.FieldId = newData.FieldId ?? toEdit.FieldId;
            toEdit.Description = newData.Description ?? toEdit.Description;
            toEdit.Name = newData.Name ?? toEdit.Name;
            toEdit.Value = newData.Value ?? toEdit.Value;

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
        public async Task<Result<Item>> GetFromIdAsync(Guid itemId)
        {
            Item? item = await _dbContext.Items.FirstOrDefaultAsync(it => it.Id == itemId);

            if(item == null)
            {
                return Result<Item>.Failure("item wasnt found" , statusCode: HttpStatusCode.NotFound);
            }

            return Result<Item>.Success(item);
        }
        public async Task<Result> RemoveByIdAsync(Guid itemId)
        {
            Item? toDel = await _dbContext.Items.FirstOrDefaultAsync(it => it.Id == itemId);

            if (toDel == null)
            {
                return Result.Failure("item wasnt Found" , statusCode: HttpStatusCode.NotFound);
            }

            _dbContext.Items.Remove(toDel);
            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }
}
