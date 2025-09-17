namespace RideSharing.API.Models.Domain
{
    public class Driver
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public int SeatingCapacity { get; set; }
        public string KycStatus { get; set; }
        public string? KycDocUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Users User { get; set; }
        public ICollection<Route> Routes { get; set; }
        public ICollection<Payout> Payouts { get; set; }
    }
}
