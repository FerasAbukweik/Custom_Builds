using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IProductServices;

namespace Custom_Builds.Core.Services.CartItemServices
{
    public class AddCartItemService : IAddCartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IGetProductService _getProductService;
        private readonly IAddCustomBuildService _addCustomBuildService;
        private readonly ICurrTokenService _getCurrUserService;
        private readonly ICustomBuildRepository _customBuildRepository;

        public AddCartItemService(ICartItemRepository cartItemRepository,
                                  IGetProductService getProductService,
                                  IAddCustomBuildService customBuildService,
                                  ICurrTokenService getCurrUserService,
                                  ICustomBuildRepository customBuildRepository)
        {
            _cartItemRepository = cartItemRepository;
            _getProductService = getProductService;
            _addCustomBuildService = customBuildService;
            _getCurrUserService = getCurrUserService;
            _customBuildRepository = customBuildRepository;
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
                Quantity = 1,
                AddedAt = DateTime.UtcNow,
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
            // get current userId
            var getCurrentUserId = _getCurrUserService.GetUserId();
            if (!getCurrentUserId.IsSuccess) getCurrentUserId.MapFailure<CartItemDTO>();

            // make new custom build based on List<Modification> in the customBuild table so we can link it with cart item
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<CartItemDTO>();

            // new cart item to add
            CartItem newCartItem = new CartItem()
            {
                Id = Guid.NewGuid(),
                orderType = OrderTypeEnum.Custom,
                UserId = getCurrentUserId.Value,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value!.Id,
                AddedAt = DateTime.UtcNow,
            };

            // adding item to the cart
            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<CartItemDTO>();


            // get modifications price to add it to the dto
            var getModificaitonsPrice = await _customBuildRepository.GetPriceAsync(addCustomBuildResult.Value!.Id);
            if (!getModificaitonsPrice.IsSuccess) return getModificaitonsPrice.MapFailure<CartItemDTO>();

            return Result<CartItemDTO>.Success(addToCartResult.Value!.toDTO(getModificaitonsPrice.Value!));
        }
    }
}