using Custom_Builds.Core.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditFieldDTO
    {
        [Required(ErrorMessage = "{0} Is requiered")]
        public required Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public Guid? SectionId { get; set; }
    }
}
