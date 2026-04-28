using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.Domain.RepositoryContracts
{
    public interface ISectionRepository
    {
        Task<Result<Section>> GetByIdAsync(Guid sectionId);
        Task<Result> RemoveByIdAsync(Guid sectionId);
        Task<Result> EditByIdAsync(EditSectionDTO newData);
        Task<Result> AddAsync(AddSectionDTO toAdd);
    }
}
