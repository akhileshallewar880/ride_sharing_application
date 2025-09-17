namespace RideSharing.API.Models.DTO
{
    public record CreatePayoutRequest(Guid DriverId, DateTime PeriodStart, DateTime PeriodEnd);
    public record PayoutDto(Guid Id, Guid DriverId, decimal Amount, string Status, DateTime CreatedAt);
}
