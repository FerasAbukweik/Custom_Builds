using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class EditItemService : IEditItemService
    {
        private readonly IItemsRepository _itemsRepository;

        public EditItemService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public Task<Result> EditByIdAsync(EditItemDTO newData)
        {
            return _itemsRepository.EditByIdAsync(newData);
        }
    }
}
