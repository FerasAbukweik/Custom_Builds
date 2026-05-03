using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.OrderServices
{
    public interface IAddOrderService
    {
        Task<Result<OrderDTO>> AddAsync(AddOrderDTO toAdd);
        Task<Result<OrderDTO>> AddCustomBuildAsync(AddCustomBuildDTO toAdd);
    }
}
