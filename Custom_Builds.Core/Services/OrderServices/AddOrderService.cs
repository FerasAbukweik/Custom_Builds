using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CartItemServices;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.ICartItemServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using Custom_Builds.Core.ServiceContracts.OrderServices;
using System.Net;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class AddOrderService : IAddOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAddCustomBuildService _addCustomBuildService;
        private readonly IGetProductService _getProductService;
        private readonly IGetCartItemService _getCartItemRepository;
        private readonly IAddCartItemService _addCartItemService;
        private readonly IGetModificationService _getModificationsService;

        public AddOrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IAddCustomBuildService addCustomBuildService,
            IGetProductService getProductService,
            IGetCartItemService getCartItemRepository,
            IAddCartItemService addCartItemService,
            IGetModificationService getModificationsService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _addCustomBuildService = addCustomBuildService;
            _getProductService = getProductService;
            _getModificationsService = getModificationsService;
            _addCartItemService = addCartItemService;
            _getCartItemRepository = getCartItemRepository;
        }

        public async Task<Result<Guid>> AddAsync(AddOrderDTO toAdd)
        {
            var getProductResult = await _getProductService.GetByIdAsync(toAdd.ProductId);

            if (!getProductResult.IsSuccess)
            {
                return Result<Guid>.Failure(getProductResult.ErrorMessage ?? "Product not found", getProductResult.StatusCode);
            }

            decimal price = getProductResult.Value?.Price ?? -1m;

            if (price == -1m)
            {
                return Result<Guid>.Failure("Product price not found", HttpStatusCode.InternalServerError);
            }

            AddOrderTO_DB newCartItem = new AddOrderTO_DB()
            {
                OrderType = OrderTypeEnum.Product,
                UserId = toAdd.UserId,
                TotalPrice = price,
                ProductId = toAdd.ProductId,
                CustomBuildId = null
            };

            var addToCartResult = await _orderRepository.AddAsync(newCartItem);

            if (!addToCartResult.IsSuccess)
            {
                return Result<Guid>.Failure(addToCartResult.ErrorMessage ?? "Failed to add item to cart", addToCartResult.StatusCode);
            }

            return Result<Guid>.Success(addToCartResult.Value);
        }
        public async Task<Result<Guid>> AddCustomBuildAsync(AddCustomBuildDTO toAdd)
        {
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd.ModificationIds, toAdd.CustomBuildType);

            if (!addCustomBuildResult.IsSuccess)
            {
                return Result<Guid>.Failure(addCustomBuildResult.ErrorMessage ?? "Cannt add custom build to DB", addCustomBuildResult.StatusCode);
            }

            var getPriceResult = await _getModificationsService.GetModificationsPriceAsync(toAdd.ModificationIds);

            if (!getPriceResult.IsSuccess)
            {
                return Result<Guid>.Failure(getPriceResult.ErrorMessage ?? "Failed to calculate price for custom build", getPriceResult.StatusCode);
            }

            AddOrderTO_DB newCartItem = new AddOrderTO_DB()
            {
                UserId = toAdd.UserId,
                TotalPrice = getPriceResult.Value,
                OrderType = OrderTypeEnum.Custom,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value
            };

            var addToCartResult = await _orderRepository.AddAsync(newCartItem);

            if (!addToCartResult.IsSuccess)
            {
                return Result<Guid>.Failure(addToCartResult.ErrorMessage ?? "Failed to add item to cart", addToCartResult.StatusCode);
            }

            return Result<Guid>.Success(addToCartResult.Value);
        }
    }
}
