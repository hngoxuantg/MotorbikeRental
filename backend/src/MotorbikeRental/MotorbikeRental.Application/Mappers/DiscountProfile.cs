using AutoMapper;
using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Application.Mappers
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountCreateDto, Discount>();
            CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.CategoryNames,
                opt => opt.MapFrom(src => src.Categories.Where(dc => dc.Category != null).Select(dc => dc.Category.CategoryName)));
            CreateMap<DiscountUpdateDto, Discount>();
        }
    }
}
