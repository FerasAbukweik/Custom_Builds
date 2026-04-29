using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class AddItemService : IAddItemService
    {
        private readonly IItemsRepository _itemsRepository;

        public AddItemService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public Task<Result> AddAsync(AddItemDTO toAdd)
        {
            return _itemsRepository.AddAsync(toAdd);
        }
    }
}
