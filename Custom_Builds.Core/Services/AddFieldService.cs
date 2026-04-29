using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts;

namespace Custom_Builds.Core.Services
{
    public class AddFieldService : IAddFieldService
    {
        private readonly IFieldRepository _fieldRepository;

        public AddFieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public Task<Result> AddAsync(AddFieldDTO toAdd)
        {
            return _fieldRepository.AddAsync(toAdd);
        }
    }
}
