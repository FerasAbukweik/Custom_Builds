using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RemoveItemService : IRemoveItemService
    {
        private readonly IItemsRepository _itemsRepository;

        public RemoveItemService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public Task<Result> RemoveByIdAsync(Guid itemId)
        {
            return _itemsRepository.RemoveByIdAsync(itemId);
        }
    }
}
