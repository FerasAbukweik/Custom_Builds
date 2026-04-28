using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace Custom_Builds.Core.Domain.Entities
{
    public class Section
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        [Column(TypeName = "varchar")]
        public required string Icon { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Name { get; set; }



        public List<Field> Fields= new List<Field>();
    }
}