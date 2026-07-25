namespace Custom_Builds.Core.DTO.Cart;

public class Id_Quantity_DTO
{
    public required Guid ItemId { get; set; }
    public required int NewQuantity { get; set; }
}