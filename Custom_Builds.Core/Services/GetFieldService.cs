using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class GetFieldService : IGetFieldService
    {
        private readonly IFieldRepository _fieldRepository;

        public GetFieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public Task<Result<Field>> GetByIdAsync(Guid fieldId)
        {
            return _fieldRepository.GetByIdAsync(fieldId);
        }
    }
}
