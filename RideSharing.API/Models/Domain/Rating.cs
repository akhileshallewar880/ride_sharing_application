namespace RideSharing.API.Models.Domain
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        public byte RatingValue { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Booking Booking { get; set; }
    }
}
