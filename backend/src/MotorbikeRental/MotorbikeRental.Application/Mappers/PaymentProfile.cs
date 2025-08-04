using AutoMapper;
using MotorbikeRental.Application.DTOs.Payments;
using MotorbikeRental.Domain.Entities.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Mappers
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.RentalContract.Customer.FullName))
                .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FullName));
        }
    }
}
