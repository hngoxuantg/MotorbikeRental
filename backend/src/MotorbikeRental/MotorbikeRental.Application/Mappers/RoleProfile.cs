using AutoMapper;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Mappers
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Roles, RoleDto>()
                .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Name));
        }
    }
}
