using Custom_Builds.Core.Enums;

namespace Custom_Builds.Core.DTO.Order;

public class OrderDetailsDto
{
    public required Guid Id { get; set; }
    public required DateTime OrderedDate { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public required Guid UserId { get; set; }
    public required OrderStateEnum Status { get; set; }
}