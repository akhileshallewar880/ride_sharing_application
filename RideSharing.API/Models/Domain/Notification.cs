namespace RideSharing.API.Models.Domain
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Users? User { get; set; }
    }
}
