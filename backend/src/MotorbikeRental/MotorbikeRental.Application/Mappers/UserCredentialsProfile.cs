using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Application.DTOs.AuthenticDto;

namespace MotorbikeRental.Application.Mappers
{
    public class UserCredentialsProfile : Profile
    {
        public UserCredentialsProfile()
        {
            CreateMap<UserCredentialsCreateDto, UserCredentials>();
            CreateMap<UserCredentials, UserCredentialsDto>()
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Roles.Name))
                .ForMember(dest => dest.EmployeeId,
                opt => opt.MapFrom(src => src.Employee.EmployeeId))
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<UserCredentialsUpdateDto, UserCredentials>();
        }
    }
}
