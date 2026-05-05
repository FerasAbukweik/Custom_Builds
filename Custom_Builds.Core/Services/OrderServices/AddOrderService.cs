using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.CustomBuildServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
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
        private readonly IGetCurrUserService _getCurrUserService;

        public AddOrderService(
            IOrderRepository orderRepository,
            IAddCustomBuildService addCustomBuildService,
            IGetProductService getProductService,
            IGetModificationService getModificationsService,
            IGetCurrUserService getCurrUserService)
        {
            _orderRepository = orderRepository;
            _addCustomBuildService = addCustomBuildService;
            _getProductService = getProductService;
            _getModificationsService = getModificationsService;
            _getCurrUserService = getCurrUserService;
        }

        public async Task<Result<OrderDTO>> AddAsync(AddOrderDTO toAdd)
        {
            // get product so we can access its price and title
            var getProductResult = await _getProductService.GetByIdAsync(toAdd.ProductId);
            if (!getProductResult.IsSuccess) return getProductResult.MapFailure<OrderDTO>();

            // new cart item
            Order newCartItem = new Order()
            {
                Id = Guid.NewGuid(),
                OrderType = OrderTypeEnum.Product,
                UserId = toAdd.UserId,
                ProductId = toAdd.ProductId,
                CustomBuildId = null,
                TotalPrice = getProductResult.Value!.Price,
                Title = getProductResult.Value!.Name,
                CreatedAt = DateTime.UtcNow,
            };

            // add item to cart table
            var addToCartResult = await _orderRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<OrderDTO>();

            return Result<OrderDTO>.Success(addToCartResult.Value!.toDTO());
        }
    }
}
