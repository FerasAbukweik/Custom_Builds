using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Custom_Builds.Core.DTO.Modification;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Modification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        [Required]
        public required decimal Price { get; set; }

        
        // relations
        [Required]
        public required Guid SectionId { get; set; }
        public Section? Section { get; set; }
        public List<CustomBuild> CustomBuilds { get; set; } = [];
        
        [Required]
        public required Guid ImageId { get; set; }
        public Image? Image { get; set; }


        
        // DTO
        public ModificationDTO toDTO()
        {
            return new ModificationDTO()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Image = Image?.ImageUrl ?? ""
            };
        }
    }
}