using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IProductServices
{
    public interface IRemoveProductService { Task<Result> RemoveByIdAsync(Guid productId); }
}
