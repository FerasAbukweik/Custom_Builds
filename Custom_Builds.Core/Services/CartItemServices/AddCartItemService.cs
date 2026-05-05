using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class AddCartItemService : IAddCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetProductService _getProductService;
        private readonly IAddCustomBuildService _addCustomBuildService;
        private readonly IGetCurrUserService _getCurrUserService;

        public AddCartItemService(ICartItemRepository cartItemRepository,
                                  IGetProductService getProductService,
                                  IAddCustomBuildService customBuildService,
                                  IGetCurrUserService getCurrUserService)
        {
            _cartItemRepository = cartItemRepository;
            _getProductService = getProductService;
            _addCustomBuildService = customBuildService;
            _getCurrUserService = getCurrUserService;
        }

        public async Task<Result<CartItemDTO>> AddAsync(Guid productId)
        {
            // get current userId
            var getCurrentUserId = _getCurrUserService.GetUserId();
            if (!getCurrentUserId.IsSuccess) getCurrentUserId.MapFailure<CartItemDTO>();

            // new item to add
            CartItem newCartItem = new CartItem()
            {
                Id = Guid.NewGuid(),
                orderType = OrderTypeEnum.Product,
                UserId = getCurrentUserId.Value!,
                ProductId = productId,
            };

            // adding item to the cart
            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<CartItemDTO>();

            // get produce so we can access its price
            var getProductResult = await _getProductService.GetByIdAsync(newCartItem.ProductId!.Value);
            if (!getProductResult.IsSuccess) return getProductResult.MapFailure<CartItemDTO>();


            return Result<CartItemDTO>.Success(newCartItem.toDTO(getProductResult.Value!.Price));
        }
        public async Task<Result<CartItemDTO>> AddCustomBuildAsync(AddCustomBuildDTO toAdd)
        {
            // get target userId
            var getCurrnetUserIdRes = _getCurrUserService.GetUserId();
            if (!getCurrnetUserIdRes.IsSuccess) getCurrnetUserIdRes.MapFailure<CartItemDTO>();


            // make new custom build based on List<Modification> in the customBuild table so we can link it with cart item
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<CartItemDTO>();

            // new cart item to add
            CartItem newCartItem = new CartItem()
            {
                Id = Guid.NewGuid(),
                orderType = OrderTypeEnum.Custom,
                UserId = getCurrnetUserIdRes.Value,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value!.Id
            };

            // adding item to the cart
            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<CartItemDTO>();


            // get item price to add it to the dto
            var getCartItemResult = await _getProductService.GetByIdAsync(newCartItem.ProductId!.Value);
            if (!getCartItemResult.IsSuccess) return getCartItemResult.MapFailure<CartItemDTO>();

            return Result<CartItemDTO>.Success(addToCartResult.Value!.toDTO(getCartItemResult.Value!.Price));
        }
    }
}