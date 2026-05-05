using Custom_Builds.Core.DTO;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Section
    {
        [Key]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Title { get; set; }

        public List<Modification> Modifications = new List<Modification>();


        public List<Part> Parts = new List<Part>();


        public SectionDTO toDTO()
        {
            return new SectionDTO()
            {
                Id = this.Id,
                Title = this.Title,
                Modifications = this.Modifications
            };
        }
    }
}
