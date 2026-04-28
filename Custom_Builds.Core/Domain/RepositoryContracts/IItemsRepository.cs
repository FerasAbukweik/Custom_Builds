using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IItemsRepository
    {
        Task<Result<Item>> GetFromIdAsync(Guid itemId);
        Task<Result> RemoveByIdAsync(Guid itemId);
        Task<Result> EditByIdAsync(EditItemDTO newData);
        Task<Result> AddAsync(AddItemDTO toAdd);
    }
}
