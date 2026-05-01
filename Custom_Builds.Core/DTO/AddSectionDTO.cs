using Custom_Builds.Core.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.DTO
{
    public class AddSectionDTO
    {
        [Required(ErrorMessage = "{0} Is Requiered")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "{0} Is Requiered")]
        public required Guid PartId { get; set; }
    }
}
