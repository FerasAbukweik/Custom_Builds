using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Reqiered")]
        public required string Name { get; set; }

        [Column(TypeName = "varchar")]
        public string? Value { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "varchar")]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Reqiered")]
        public required decimal Price { get; set; }



        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid FieldId { get; set; }
        public Field? Field { get; set; }
    }
}