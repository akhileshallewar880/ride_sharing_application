namespace RideSharing.API.Models.Domain
{
    public class Payout
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Driver Driver { get; set; }
    }
}
