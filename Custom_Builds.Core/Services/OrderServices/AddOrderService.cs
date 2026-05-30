using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IProductServices;
using Custom_Builds.Core.ServiceContracts.OrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class AddOrderService : IAddOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGetProductService _getProductService;
        private readonly ICurrTokenService _currUserService;

        public AddOrderService(
            IOrderRepository orderRepository,
            IGetProductService getProductService,
            ICurrTokenService currUserService)
        {
            _orderRepository = orderRepository;
            _getProductService = getProductService;
            _currUserService = currUserService;
        }

        public async Task<Result<OrderDTO>> AddProductAsync(AddOrderDTO toAdd)
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
                Title = getProductResult.Value!.Name,
                CreatedAt = DateTime.UtcNow,
                TotalPrice = getProductResult.Value.Price * toAdd.Quantity,
                Quantity = toAdd.Quantity,
                OrderStatus = OrderStateEnum.Processing,
            };

            // add item to cart table
            var addToCartResult = await _orderRepository.AddAsync(newCartItem);
            if (!addToCartResult.IsSuccess) return addToCartResult.MapFailure<OrderDTO>();

            return Result<OrderDTO>.Success(addToCartResult.Value!.toDTO());
        }
        public async Task<Result> BuyAgain(Guid OrderId)
        {
            var getCurrUserIdRes = _currUserService.GetUserId();
            if (!getCurrUserIdRes.IsSuccess) return getCurrUserIdRes;

            var addOrderResult = await _orderRepository.BuyAgainAsync(OrderId, getCurrUserIdRes.Value);

            return addOrderResult;
        }
    }
}
