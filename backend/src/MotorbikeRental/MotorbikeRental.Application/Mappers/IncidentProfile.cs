using AutoMapper;
using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Domain.Entities.Incidents;

namespace MotorbikeRental.Application.Mappers
{
    public class IncidentProfile : Profile
    {
        public IncidentProfile()
        {
            CreateMap<IncidentCreateDto, Incident>()
                .ForMember(dest => dest.IsResolved,
                opt => opt.MapFrom(src => src.ResolvedDate.HasValue ? true : false))
                .ForMember(dest => dest.Images,
                opt => opt.Ignore());
            CreateMap<Incident, IncidentDto>()
                .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(e => e.ImageUrl)));
            CreateMap<IncidentUpdateBeforeCompleteDto, Incident>()
                .ForMember(dest => dest.IsResolved,
                opt => opt.MapFrom(src => src.ResolvedDate.HasValue ? true : false))
                .ForMember(dest => dest.Images,
                opt => opt.Ignore());
            CreateMap<Incident, IncidentListDto>()
                .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(e => e.ImageUrl)));
        }
    }
}