using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface IFieldRepository
    {
        Task<Result<Field>> GetByIdAsync(Guid fieldId);
        Task<Result> RemoveByIdAsync(Guid fieldId);
        Task<Result> EditByIdAsync(EditFieldDTO newData);
        Task<Result> AddAsync(AddFieldDTO toAdd);
    }
}
