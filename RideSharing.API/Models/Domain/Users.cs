using System.ComponentModel.DataAnnotations;

namespace RideSharing.API.Models.Domain
{
    public class Users
    {
        public Guid Id { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Driver? Driver { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
