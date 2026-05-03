using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IOrderServices;

namespace Custom_Builds.Core.Services.OrderServices
{
    public class EditOrderService : IEditOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public EditOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> EditByIdAsync(EditOrderDTO newData)
        {
            var result = await _orderRepository.EditByIdAsync(newData);

            return result;
        }
    }
}
