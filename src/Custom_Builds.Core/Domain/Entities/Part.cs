using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Custom_Builds.Core.DTO.Part;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Part
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string Icon { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public required string Name { get; set; }

        
        // relations
        public List<Section> Sections= new List<Section>();


        // dto
        // must include sections
        public PartDTO toDTO()
        {
            return new PartDTO()
            {
                Id = Id,
                Icon = Icon,
                Name = Name,
                Sections = Sections.Select(s => s.toDTO()).ToList()
            };
        }
    }
}