namespace RideSharing.API.Models.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid RideInstanceId { get; set; }
        public Guid PassengerUserId { get; set; }
        public int SeatsBooked { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Navigation
        public RideInstance RideInstance { get; set; }
        public Users PassengerUser { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
