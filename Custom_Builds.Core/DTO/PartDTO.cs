using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Custom_Builds.Core.DTO
{
    public class PartDTO
    {
        public Guid Id { get; set; }
        public required string Icon { get; set; }
        public required string Name { get; set; }
    }
}
