using Custom_Builds.Core.DTO;
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

        [Column(TypeName = "varchar(150)")]
        public string? Value { get; set; }
        
        [Column(TypeName = "varchar(150)")]
        public string? Description { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Icon { get; set; }

        [Required]
        public required decimal Price { get; set; }

        
        // relations
        [Required]
        public required Guid SectionId { get; set; }
        public Section? Section { get; set; }
        public List<CustomBuild> CustomBuilds { get; set; } = [];


        
        // DTO
        public ModificationDTO toDTO()
        {
            return new ModificationDTO()
            {
                Id = Id,
                Description = Description,
                Name = Name,
                Price = Price,
                Icon = Icon,
                Image = Value
            };
        }
    }
}