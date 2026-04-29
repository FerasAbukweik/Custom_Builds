using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class GetItemService : IGetItemService
    {
        private readonly IItemsRepository _itemsRepository;

        public GetItemService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public Task<Result<Item>> GetFromIdAsync(Guid itemId)
        {
            return _itemsRepository.GetFromIdAsync(itemId);
        }
    }
}
