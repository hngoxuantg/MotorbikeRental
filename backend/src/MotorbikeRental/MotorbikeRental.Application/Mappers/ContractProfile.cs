using AutoMapper;
using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Domain.Entities.Contract;

namespace MotorbikeRental.Application.Mappers
{
    public class ContractProfile : Profile
    {
        public ContractProfile()
        {
            CreateMap<ContractCreateDto, RentalContract>()
                .ForMember(dest => dest.RentalContractStatus,
                opt => opt.MapFrom(src => src.Status));
            CreateMap<RentalContract, ContractDto>();
            CreateMap<ContractUpdateBeforeActivationDto, RentalContract>();
            CreateMap<RentalContract, ContractListDto>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.RentalContractStatus))
                .ForMember(dest => dest.RentalTypeStatus,
                opt => opt.MapFrom(src => src.RentalTypeStatus))
                .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.MotorbikeName,
                opt => opt.MapFrom(src => src.Motorbike.MotorbikeName))
                .ForMember(dest => dest.ImageUrlMotorbike,
                opt => opt.MapFrom(src => src.Motorbike.ImageUrl))
                .ForMember(dest => dest.DiscountName,
                opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Name : null));
        }
    }
}
