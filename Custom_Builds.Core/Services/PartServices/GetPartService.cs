using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;

namespace Custom_Builds.Core.Services.PartServices
{
    public class GetPartService : IGetPartService
    {
        private readonly IPartRepository _partRepository;

        public GetPartService(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<Result<Part>> GetByIdAsync(Guid partId)
        {
            var result = await _partRepository.GetByIdAsync(partId);
            if (!result.IsSuccess) return result.MapFailure<Part>();

            return Result<Part>.Success(result.Value!);
        }
        public async Task<Result<List<Part>>> GetAllPartsIncludingAllData()
        {
            return await _partRepository.GetAllPartsIncludingAllData();
        }
    }
}
