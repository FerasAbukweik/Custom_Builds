using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using System.Globalization;
using System.Net;

namespace Custom_Builds.Core.Services.ModificationServices
{
    public class GetModificationService : IGetModificationService
    {
        private readonly IModificationsRepository _modificationsRepository;

        public GetModificationService(IModificationsRepository modificationsRepository)
        {
            _modificationsRepository = modificationsRepository;
        }

        public async Task<Result<Modification>> GetFromIdAsync(Guid modificationId)
        {
            var result = await _modificationsRepository.GetFromIdAsync(modificationId);
            if (!result.IsSuccess) return result.MapFailure<Modification>();

            return Result<Modification>.Success(result.Value!);
        }
    }
}
