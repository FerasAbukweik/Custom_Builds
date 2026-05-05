using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ModificationServices;

namespace Custom_Builds.Core.Services.ModificationServices
{
    public class AddModificationService : IAddModificationService
    {
        private readonly IModificationsRepository _modificationsRepository;
        private readonly ISectionRepository _sectionRepository;

        public AddModificationService(IModificationsRepository modificationsRepository,
                                      ISectionRepository sectionRepository)
        {
            _modificationsRepository = modificationsRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<Result<ModificationDTO>> AddAsync(AddModificationDTO toAdd)
        {
            // get Section id for modification i wanna add
            var getSectionResult = await _sectionRepository.GetByIdAsync(toAdd.SectionId);
            if (!getSectionResult.IsSuccess) return getSectionResult.MapFailure<ModificationDTO>();

            // new modification to add
            Modification newModification = new Modification()
            {
                Id = Guid.NewGuid(),
                Name = toAdd.Name,
                Price = toAdd.Price,
                Type = toAdd.Type,
                Description = toAdd.Description,
                Icon = toAdd.Icon,
                Value = toAdd.Value,
                Sections = [getSectionResult.Value!]
            };

            // adding the modification to the DB
            var result = await _modificationsRepository.AddAsync(newModification);
            if(!result.IsSuccess) return result.MapFailure<ModificationDTO>();

            return Result<ModificationDTO>.Success(newModification.toDTO());
        }
    }
}
