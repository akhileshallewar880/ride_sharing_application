namespace RideSharing.API.Models.DTO
{
    public record NotificationDto(
        Guid Id,
        Guid? UserId,
        string Type,
        string Payload,
        DateTime? SentAt
    );
}
