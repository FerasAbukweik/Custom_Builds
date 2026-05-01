using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IProductServices
{
    public interface IEditProductService { Task<Result> EditByIdAsync(EditProductDTO newData); }
}
