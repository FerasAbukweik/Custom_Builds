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

        public async Task<Result<Guid>> AddAsync(AddPartDTO toAdd)
        {
            var result = await _partRepository.AddAsync(toAdd);

            if (result.IsSuccess)
            {
                return Result<Guid>.Failure(result.ErrorMessage ?? "An error occurred while adding the part.", result.StatusCode);
            }

            return Result<Guid>.Success(result.Value);

        }
    }
}
