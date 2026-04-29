using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class EditFieldService : IEditFieldService
    {
        private readonly IFieldRepository _fieldRepository;

        public EditFieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public Task<Result> EditByIdAsync(EditFieldDTO newData)
        {
            return _fieldRepository.EditByIdAsync(newData);
        }
    }
}
