using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Section
    {
        [Key]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Title { get; set; }

        public List<Modification> Modifications = new List<Modification>();


        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid PartId { get; set; }
        public Part? Part { get; set; }
    }
}
