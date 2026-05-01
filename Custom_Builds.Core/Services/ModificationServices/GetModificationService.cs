using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
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
            return await _modificationsRepository.GetFromIdAsync(modificationId);
        }

        public async Task<Result<decimal>> GetModificationsPriceAsync(List<Guid> modificationIds)
        {
            var modificationsResult = await _modificationsRepository.GetListFromIdsAsync(modificationIds);
            if (!modificationsResult.IsSuccess)
            {
                return Result<decimal>.Failure(modificationsResult.ErrorMessage ?? "Failed to get modifications", modificationsResult.StatusCode);
            }

            if (modificationsResult.Value == null || modificationsResult.Value.Count != modificationIds.Count)
            {
                return Result<decimal>.Failure("One or more modifications were not found", HttpStatusCode.NotFound);
            }

            decimal totalPrice = modificationsResult.Value.Sum(m => m.Price);
            return Result<decimal>.Success(totalPrice);
        }
    }
}
