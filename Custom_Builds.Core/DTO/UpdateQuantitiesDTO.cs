using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class UpdateQuantitiesDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        public required Dictionary<Guid, int> newQiantities { get; set; }
    }
}
