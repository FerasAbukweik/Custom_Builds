using Custom_Builds.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Custom_Builds.Core.DTO
{
    public class SectionDTO
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required List<Modification> Modifications { get; set; }
    }
}
