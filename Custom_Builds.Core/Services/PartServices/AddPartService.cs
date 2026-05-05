using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.PartServices;

namespace Custom_Builds.Core.Services.PartServices
{
    public class AddPartService : IAddPartService
    {
        private readonly IPartRepository _partRepository;

        public AddPartService(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<Result<PartDTO>> AddAsync(AddPartDTO toAdd)
        {
            // new part
            Part newPart = new Part()
            {
                Id = Guid.NewGuid(),
                Name = toAdd.Name,
                Icon = toAdd.Icon,
            };

            var result = await _partRepository.AddAsync(newPart);
            if (!result.IsSuccess) return result.MapFailure<PartDTO>();

            return Result<PartDTO>.Success(result.Value!.toDTO());
        }
    }
}
