using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                    .ForMember(dest => dest.RentalCount,
                    opt => opt.MapFrom(src => src.RentalContracts.Count(e => e.RentalContractStatus == RentalContractStatus.Completed)));
            CreateMap<CustomerDto, Customer>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<Customer, CustomerListDto>()
                    .ForMember(dest => dest.RentalCount,
                    opt => opt.MapFrom(src => src.RentalContracts.Count(e => e.RentalContractStatus == RentalContractStatus.Completed)));
            CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
