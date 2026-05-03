using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IPartServices;

namespace Custom_Builds.Core.Services.PartServices
{
    public class EditPartService : IEditPartService
    {
        private readonly IPartRepository _partRepository;

        public EditPartService(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<Result> EditByIdAsync(EditPartDTO newData)
        {
            var result = await _partRepository.EditByIdAsync(newData);
            if (!result.IsSuccess) return result.MapFailure();

            return Result.Success();
        }
    }
}
