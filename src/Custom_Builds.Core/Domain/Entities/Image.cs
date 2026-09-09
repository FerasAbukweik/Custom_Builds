using System.ComponentModel.DataAnnotations;
using Custom_Builds.Core.Domain.Identity;

namespace Custom_Builds.Core.Domain.Entities;

public class Image
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public required string ImageUrl { get; set; }

    [Required] public required string PublicId { get; set; }
    
    
    // relations
    
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Modification? Modification { get; set; }

    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}