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
    public class RoutesSchedulesRepository : IRoutesSchedulesRepository
    {
        private readonly RideSharingDbContext _db;

        public RoutesSchedulesRepository(RideSharingDbContext db)
        {
            _db = db;
        }

        public async Task<RouteDto?> CreateRouteAsync(CreateRouteRequest req)
        {
            var driver = await _db.Drivers.FindAsync(req.DriverId);
            if (driver == null) return null;

            var route = new Models.Domain.Route
            {
                Id = Guid.NewGuid(),
                DriverId = req.DriverId,
                RouteName = req.RouteName,
                Origin = req.Origin,
                Destination = req.Destination,
                DistanceKm = req.DistanceKm,
                CreatedAt = DateTime.UtcNow,
                RouteStops = req.Stops?.Select(s => new RouteStop
                {
                    Id = Guid.NewGuid(),
                    StopOrder = s.StopOrder,
                    StopName = s.StopName,
                    StopLat = s.StopLat,
                    StopLng = s.StopLng
                }).ToList()
            };

            _db.Routes.Add(route);
            await _db.SaveChangesAsync();

            return new RouteDto(
                route.Id, route.DriverId, route.RouteName, route.Origin, route.Destination, route.DistanceKm,
                route.RouteStops?.OrderBy(s => s.StopOrder).Select(s => new RouteStopDto(s.StopOrder, s.StopName, s.StopLat, s.StopLng))
            );
        }

        public async Task<RouteDto?> GetRouteAsync(Guid id)
        {
            var route = await _db.Routes
                .Include(r => r.RouteStops)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null) return null;

            return new RouteDto(
                route.Id, route.DriverId, route.RouteName, route.Origin, route.Destination, route.DistanceKm,
                route.RouteStops?.OrderBy(s => s.StopOrder).Select(s => new RouteStopDto(s.StopOrder, s.StopName, s.StopLat, s.StopLng))
            );
        }

        public async Task<IEnumerable<RouteDto>> SearchRoutesAsync(string origin, string destination)
        {
            var query = _db.Routes
                .Include(r => r.RouteStops)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(origin))
                query = query.Where(r => r.Origin == origin);

            if (!string.IsNullOrWhiteSpace(destination))
                query = query.Where(r => r.Destination == destination);

            var routes = await query.ToListAsync();

            return routes.Select(route => new RouteDto(
                route.Id, route.DriverId, route.RouteName, route.Origin, route.Destination, route.DistanceKm,
                route.RouteStops?.OrderBy(s => s.StopOrder).Select(s => new RouteStopDto(s.StopOrder, s.StopName, s.StopLat, s.StopLng))
            ));
        }

        public async Task<ScheduleTemplateDto?> CreateScheduleTemplateAsync(Guid routeId, CreateScheduleTemplateRequest req)
        {
            if (routeId != req.RouteId) return null;

            var route = await _db.Routes.FindAsync(routeId);
            if (route == null) return null;

            var template = new ScheduleTemplate
            {
                Id = Guid.NewGuid(),
                RouteId = routeId,
                DaysOfWeek = req.DaysOfWeekMask,
                DepartureTime = req.DepartureTime,
                Capacity = req.Capacity,
                PricePerSeat = req.PricePerSeat,
                IsAutoConfirm = req.IsAutoConfirm,
                CreatedAt = DateTime.UtcNow
            };

            _db.ScheduleTemplates.Add(template);
            await _db.SaveChangesAsync();

            return new ScheduleTemplateDto(
                template.Id, template.RouteId, template.DaysOfWeek, template.DepartureTime, template.Capacity, template.PricePerSeat, template.IsAutoConfirm
            );
        }

        public async Task<IEnumerable<ScheduleTemplateDto>> GetScheduleTemplatesAsync(Guid routeId)
        {
            var templates = await _db.ScheduleTemplates
                .Where(t => t.RouteId == routeId)
                .ToListAsync();

            return templates.Select(t => new ScheduleTemplateDto(
                t.Id, t.RouteId, t.DaysOfWeek, t.DepartureTime, t.Capacity, t.PricePerSeat, t.IsAutoConfirm
            ));
        }
    }
}