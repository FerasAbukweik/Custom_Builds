using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class RemoveFieldService : IRemoveFieldService
    {
        private readonly IFieldRepository _fieldRepository;

        public RemoveFieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public Task<Result> RemoveByIdAsync(Guid fieldId)
        {
            return _fieldRepository.RemoveByIdAsync(fieldId);
        }
    }
}
