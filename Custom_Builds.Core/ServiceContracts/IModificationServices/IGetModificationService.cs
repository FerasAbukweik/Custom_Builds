using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IModificationServices
{
    public interface IGetModificationService
    {
        Task<Result<Modification>> GetFromIdAsync(Guid modificationId);
        Task<Result<decimal>> GetModificationsPriceAsync(List<Guid> modificationIds);
    }
}
