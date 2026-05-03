using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.IModificationServices;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using Custom_Builds.Core.ServiceContracts.OrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class AddOrderService : IAddOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAddCustomBuildService _addCustomBuildService;
        private readonly IGetProductService _getProductService;
        private readonly IGetModificationService _getModificationsService;

        public AddOrderService(
            IOrderRepository orderRepository,
            IAddCustomBuildService addCustomBuildService,
            IGetProductService getProductService,
            IGetModificationService getModificationsService)
        {
            _orderRepository = orderRepository;
            _addCustomBuildService = addCustomBuildService;
            _getProductService = getProductService;
            _getModificationsService = getModificationsService;
        }

        public async Task<Result<OrderDTO>> AddAsync(AddOrderDTO toAdd)
        {
            // get product so we can access its price and title
            var getProductResult = await _getProductService.GetByIdAsync(toAdd.ProductId);
            if (!getProductResult.IsSuccess) return getProductResult.MapFailure<OrderDTO>();

            // new cart item
            AddOrderTODB newCartItem = new AddOrderTODB()
            {
                OrderType = OrderTypeEnum.Product,
                UserId = toAdd.UserId,
                ProductId = toAdd.ProductId,
                CustomBuildId = null,
                TotalPrice = getProductResult.Value!.Price,
                Title = getProductResult.Value!.Name
            };

            // add item to cart table
            var addToCartResult = await _orderRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<OrderDTO>();

            return Result<OrderDTO>.Success(addToCartResult.Value!.toDTO());
        }
        public async Task<Result<OrderDTO>> AddCustomBuildAsync(AddCustomBuildDTO toAdd)
        {
            // add create new custom build so we can link it with cart item
            var addCustomBuildResult = await _addCustomBuildService.AddByModificationsIdsAsync(toAdd);
            if (!addCustomBuildResult.IsSuccess) return addCustomBuildResult.MapFailure<OrderDTO>();

            // get modifications price
            var getPriceResult = await _getModificationsService.GetModificationsPriceAsync(toAdd.ModificationIds);
            if (!getPriceResult.IsSuccess) return getPriceResult.MapFailure<OrderDTO>();

            // new cart item
            AddOrderTODB newCartItem = new AddOrderTODB()
            {
                UserId = toAdd.CreatorId,
                TotalPrice = getPriceResult.Value,
                OrderType = OrderTypeEnum.Custom,
                ProductId = null,
                CustomBuildId = addCustomBuildResult.Value!.Id,
                Title = "Custom Build - " + toAdd.CustomBuildType.ToString()
            };

            // add new item to cart
            var addToCartResult = await _orderRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<OrderDTO>();

            return Result<OrderDTO>.Success(addToCartResult.Value!.toDTO());
        }
    }
}
