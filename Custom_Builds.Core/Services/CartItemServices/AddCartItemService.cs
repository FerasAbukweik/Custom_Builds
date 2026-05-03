using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class AddCartItemService : IAddCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetProductService _getProductService;
        private readonly IGetModificationService _getModificationsService;
        private readonly IAddCustomBuildService _addCustomBuildService;

        public AddCartItemService(ICartItemRepository cartItemRepository,
                                  IGetProductService getProductService,
                                  IGetModificationService getModificationsService,
                                  IAddCustomBuildService customBuildService)
        {
            _cartItemRepository = cartItemRepository;
            _getProductService = getProductService;
            _getModificationsService = getModificationsService;
            _addCustomBuildService = customBuildService;
        }

        public async Task<Result<CartItemDTO>> AddAsync(AddCartItemDTO toAdd)
        {
            // get currProduct so we can access its price
            var getProductResult = await _getProductService.GetByIdAsync(toAdd.ProductId!.Value);
            if (!getProductResult.IsSuccess) return getProductResult.MapFailure<CartItemDTO>();

            // new item to add
            AddCartItemToDB_DTO newCartItem = new AddCartItemToDB_DTO()
            {
                orderType = OrderTypeEnum.Product,
                UserId = toAdd.UserId!.Value,
                TotalPrice = getProductResult.Value!.Price,
                ProductId = toAdd.ProductId,
                CustomBuildId = null
            };

            // adding item to the cart
            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<CartItemDTO>();

            return Result<CartItemDTO>.Success(addToCartResult.Value!.toDTO());
        }
        public async Task<Result<CartItemDTO>> AddCustomBuildAsync(AddCustomBuildDTO toAdd)
        {
            // make new custom build based on List<Modification> in the customBuild table so we can link it with cart item
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<CartItemDTO>();

            // get sum of modifications prices
            var getPriceResult = await _getModificationsService.GetModificationsPriceAsync(toAdd.ModificationIds);
            if (!getPriceResult.IsSuccess) return getPriceResult.MapFailure<CartItemDTO>();

            // new cart item to add
            AddCartItemToDB_DTO newCartItem = new AddCartItemToDB_DTO()
            {
                orderType = OrderTypeEnum.Custom,
                UserId = toAdd.CreatorId,
                TotalPrice = getPriceResult.Value,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value!.Id
            };

            // adding item to the cart
            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<CartItemDTO>();

            return Result<CartItemDTO>.Success(addToCartResult.Value!.toDTO());
        }
    }
}
