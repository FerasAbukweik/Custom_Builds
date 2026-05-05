using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Modification
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Reqiered")]
        public required string Name { get; set; }

        [Column(TypeName = "varchar")]
        public string? Value { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "varchar")]
        public required string Type { get; set; }

        [Column(TypeName = "varchar")]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Reqiered")]
        public required decimal Price { get; set; }

        public List<Section> Sections { get; set; } = new List<Section>();
        public List<CustomBuild> CustomBuilds { get; set; } = new List<CustomBuild>();


        public ModificationDTO toDTO()
        {
            return new ModificationDTO()
            {
                Id = this.Id,
                Description = this.Description,
                Name = this.Name,
                Price = this.Price,
                Type = this.Type,
                Icon = this.Icon,
                Value = this.Value
            };
        }
    }
}