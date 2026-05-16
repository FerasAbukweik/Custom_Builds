using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class UpdateCartItemService : IUpdateCartItemService
    {
        private readonly ICartItemRepository _cartRepository;

        public UpdateCartItemService(ICartItemRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Result> UpdateQuantitesAsync(UpdateQuantitiesDTO newQtys)
        {
            // set for faster check for ids
            var idsSet = new HashSet<Guid>(newQtys.newQiantities.Keys);

            // get items needed to be updated
            var getCartItemsResult = await _cartRepository.FilterAsync(ci => idsSet.Contains(ci.Id));
            if (!getCartItemsResult.IsSuccess) return getCartItemsResult;

            for(int i=0; i<getCartItemsResult.Value!.Count(); i++)
            {
                getCartItemsResult.Value![i].Quantity = newQtys.newQiantities[getCartItemsResult.Value![i].Id];
            }

            var updateDBRes = await _cartRepository.UpdateRange(getCartItemsResult.Value!);


            return updateDBRes;
        }
    }
}
