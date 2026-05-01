using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using System.Net;

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

        public async Task<Result<Guid>> AddAsync(AddCartItemDTO toAdd)
        {
            var getProductResult = await _getProductService.GetByIdAsync(toAdd.ProductId);

            if (!getProductResult.IsSuccess)
            {
                return Result<Guid>.Failure(getProductResult.ErrorMessage ?? "Product not found", getProductResult.StatusCode);
            }

            decimal price = getProductResult.Value?.Price ?? -1m;

            if(price == -1m)
            {
                return Result<Guid>.Failure("Product price not found", HttpStatusCode.InternalServerError);
            }

            AddCartItemToDB_DTO newCartItem = new AddCartItemToDB_DTO()
            {
                orderType = OrderTypeEnum.Product,
                UserId = toAdd.UserId,
                TotalPrice = price,
                ProductId = toAdd.ProductId,
                CustomBuildId = null
            };

            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);

            if (!addToCartResult.IsSuccess)
            {
                return Result<Guid>.Failure(addToCartResult.ErrorMessage ?? "Failed to add item to cart", addToCartResult.StatusCode);
            }

            return Result<Guid>.Success(addToCartResult.Value);
        }
        public async Task<Result<Guid>> AddCustomBuildAsync(AddCustomBuildDTO toAdd)
        {
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd.ModificationIds , toAdd.CustomBuildType);

            if (!addCustomBuildResult.IsSuccess)
            {
                return Result<Guid>.Failure(addCustomBuildResult.ErrorMessage ?? "Cannt add custom build to DB", addCustomBuildResult.StatusCode);
            }

            var getPriceResult = await _getModificationsService.GetModificationsPriceAsync(toAdd.ModificationIds);

            if (!getPriceResult.IsSuccess)
            {
                return Result<Guid>.Failure(getPriceResult.ErrorMessage ?? "Failed to calculate price for custom build", getPriceResult.StatusCode);
            }

            AddCartItemToDB_DTO newCartItem = new AddCartItemToDB_DTO()
            {
                orderType = OrderTypeEnum.Custom,
                UserId = toAdd.UserId,
                TotalPrice = getPriceResult.Value,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value
            };

            var addToCartResult = await _cartItemRepository.AddAsync(newCartItem);

            if (!addToCartResult.IsSuccess)
            {
                return Result<Guid>.Failure(addToCartResult.ErrorMessage ?? "Failed to add item to cart", addToCartResult.StatusCode);
            }

            return Result<Guid>.Success(addToCartResult.Value);
        }
    }
}
