using System;
using AutoMapper;
using RideSharing.API.Models.Domain;
using RideSharing.API.Models.DTO;

namespace RideSharing.API.AutoMappings;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<Models.Domain.Route, RouteDto>();
        CreateMap<CreateRouteRequest, Models.Domain.Route>();
        CreateMap<ScheduleTemplate, ScheduleTemplateDto>();
        CreateMap<CreateScheduleTemplateRequest, ScheduleTemplate>();
    }

}
