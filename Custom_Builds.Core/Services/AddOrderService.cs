using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class AddOrderService : IAddOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<Result> AddAsync(AddOrderDTO toAdd)
        {
            return _orderRepository.AddAsync(toAdd);
        }
    }
}
