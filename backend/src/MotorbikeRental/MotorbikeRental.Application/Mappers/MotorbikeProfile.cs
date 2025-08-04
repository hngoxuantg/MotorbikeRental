using AutoMapper;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Domain.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Mappers
{
    public class MotorbikeProfile : Profile
    {
        public MotorbikeProfile()
        {
            CreateMap<Motorbike, MotorbikeDto>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.HourlyRate,
                opt => opt.MapFrom(src => src.PriceList.HourlyRate))
                .ForMember(dest => dest.DailyRate,
                opt => opt.MapFrom(src => src.PriceList.DailyRate));
            CreateMap<MotorbikeDto, Motorbike>();
            CreateMap<Motorbike, MotorbikeListDto>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.HourlyRate,
                opt => opt.MapFrom(src => src.PriceList.HourlyRate))
                .ForMember(dest => dest.DailyRate,
                opt => opt.MapFrom(src => src.PriceList.DailyRate));
            CreateMap<MotorbikeCreateDto, Motorbike>();
            CreateMap<MotorbikeUpdateDto, Motorbike>()
                .ForMember(dest => dest.Category,
                opt => opt.Ignore());
        }
    }
}
