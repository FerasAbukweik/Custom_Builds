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
        public async Task<Result<decimal>> GetModificationsPriceAsync(List<Guid> modificationIds)
        {
            // hashset so the comparision is faster
            HashSet<Guid> ids = new HashSet<Guid>(modificationIds);

            // get modifications based on the list of ids so we can sum their prices
            var modificationsResult = await _modificationsRepository.FilterAsync(m => ids.Contains(m.Id));
            if (!modificationsResult.IsSuccess) return modificationsResult.MapFailure<decimal>();

            if (modificationsResult.Value!.Count != modificationIds.Count)
            {
                return Result<decimal>.Failure("One or more modifications were not found", HttpStatusCode.NotFound);
            }

            // sum modifications price
            decimal totalPrice = modificationsResult.Value.Sum(m => m.Price);
            return Result<decimal>.Success(totalPrice);
        }
    }
}
