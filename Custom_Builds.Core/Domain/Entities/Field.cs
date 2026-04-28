using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Field
    {
        [Key]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        [Column(TypeName = "varchar")]
        public required string Type { get; set; }




        public List<Item> Items = new List<Item>();


        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid SectionId { get; set; }
        public Section? Section { get; set; }
    }
}
