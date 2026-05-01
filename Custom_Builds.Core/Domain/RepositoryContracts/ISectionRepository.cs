using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.Domain.RepositryContracts
{
    public interface ISectionRepository
    {
        Task<Result<Section>> GetByIdAsync(Guid sectionId);
        Task<Result<Guid>> AddAsync(AddSectionDTO toAdd);
        Task<Result> RemoveByIdAsync(Guid sectionId);
        Task<Result> EditByIdAsync(EditSectionDTO newData);
    }
}
