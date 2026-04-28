using System;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class EditSectionDTO
    {
        public required Guid Id { get; set; }
        public string? Icon { get; set; }
        public string? Name { get; set; }
    }
}
