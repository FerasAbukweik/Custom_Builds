using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class EditOrderService : IEditOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public EditOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<Result> EditByIdAsync(EditOrderDTO newData)
        {
            return _orderRepository.EditByIdAsync(newData);
        }
    }
}
