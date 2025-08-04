using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.UserCredentials.Roles.Name))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.UserCredentials.PhoneNumber))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.UserCredentials.Email));
            CreateMap<Employee, EmployeeListDto>()
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.UserCredentials.Roles.Name))
                .ForMember(dest => dest.LastLogin,
                opt => opt.MapFrom(src => src.UserCredentials.LastLogin));
        }
    }
}