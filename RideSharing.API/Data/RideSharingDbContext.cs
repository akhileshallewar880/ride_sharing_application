using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.API.Data
{
    public class RideSharingDbContext : DbContext
    {
        public RideSharingDbContext(DbContextOptions<RideSharingDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        // DbSets for all main entities
        public DbSet<Models.Domain.Users> Users { get; set; }
        public DbSet<Models.Domain.Driver> Drivers { get; set; }
        public DbSet<Models.Domain.Route> Routes { get; set; }
        public DbSet<Models.Domain.RouteStop> RouteStops { get; set; }
        public DbSet<Models.Domain.ScheduleTemplate> ScheduleTemplates { get; set; }
        public DbSet<Models.Domain.RideInstance> RideInstances { get; set; }
        public DbSet<Models.Domain.Booking> Bookings { get; set; }
        public DbSet<Models.Domain.Payment> Payments { get; set; }
        public DbSet<Models.Domain.Rating> Ratings { get; set; }
        public DbSet<Models.Domain.Notification> Notifications { get; set; }
        public DbSet<Models.Domain.Payout> Payouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade paths for Bookings
            modelBuilder.Entity<Models.Domain.Booking>()
                .HasOne(b => b.PassengerUser)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.PassengerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    
}