using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.DTO.Section;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Section
    {
        [Key]
        public required Guid Id { get; set; }

        [Required]
        public required string Title { get; set; }
        
        
        // relations
        
        [Required]
        public required Guid PartId { get; set; }
        public Part? Part { get; set; }
        public List<Modification> Modifications { get; set; } = [];


        // DTO
        // must include modifications
        public SectionDTO toDTO()
        {
            return new SectionDTO()
            {
                Id = Id,
                Title = Title,
                Modifications = Modifications.Select(m => m.toDTO()).ToList()
            };
        }
    }
}
