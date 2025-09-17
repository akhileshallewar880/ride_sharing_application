using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.API.Data;
using RideSharing.API.Models.Domain;
using RideSharing.API.Models.DTO;
using RideSharing.API.Repositories.Interface;

namespace RideSharing.API.Repositories.Implementation
{
    public class UserDriverRepository : IUserDriverRepository
    {
        private readonly RideSharingDbContext _db;

        public UserDriverRepository(RideSharingDbContext db)
        {
            _db = db;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return null;
            return new UserProfileDto(user.Id, user.Phone, user.Name, user.Email, user.Role, user.IsActive);
        }

        public async Task<UserProfileDto?> UpdateUserProfileAsync(Guid id, UpdateProfileRequest req)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return null;
            user.Name = req.Name ?? user.Name;
            user.Email = req.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return new UserProfileDto(user.Id, user.Phone, user.Name, user.Email, user.Role, user.IsActive);
        }

        public async Task<DriverDto?> RegisterDriverAsync(DriverOnboardRequest req)
        {
            var user = await _db.Users.FindAsync(req.UserId);
            if (user == null) return null;
            var driver = new Driver
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId,
                VehicleNumber = req.VehicleNumber,
                VehicleType = req.VehicleType,
                SeatingCapacity = req.SeatingCapacity,
                KycStatus = "Pending",
                KycDocUrl = req.KycDocUrl,
                CreatedAt = DateTime.UtcNow
            };
            _db.Drivers.Add(driver);
            await _db.SaveChangesAsync();
            return new DriverDto(driver.Id, driver.UserId, driver.VehicleNumber, driver.VehicleType, driver.SeatingCapacity, driver.KycStatus);
        }

        public async Task<DriverDto?> GetDriverAsync(Guid id)
        {
            var driver = await _db.Drivers.FindAsync(id);
            if (driver == null) return null;
            return new DriverDto(driver.Id, driver.UserId, driver.VehicleNumber, driver.VehicleType, driver.SeatingCapacity, driver.KycStatus);
        }

        public async Task<IEnumerable<RouteDto>> GetDriverRoutesAsync(Guid id)
        {
            var driver = await _db.Drivers
                .Include(d => d.Routes)
                .ThenInclude(r => r.RouteStops)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (driver == null) return Enumerable.Empty<RouteDto>();

            return driver.Routes.Select(r => new RouteDto(
                r.Id, r.DriverId, r.RouteName, r.Origin, r.Destination, r.DistanceKm,
                r.RouteStops.OrderBy(s => s.StopOrder).Select(s => new RouteStopDto(s.StopOrder, s.StopName, s.StopLat, s.StopLng))
            ));
        }
    }
}
