using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Application.DTOs.Pagination;

namespace MotorbikeRental.Application.Interface.IServices.IDiscountServices
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscount(DiscountCreateDto discountCreateDto);
        Task<DiscountDto> GetDiscountById(int id, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<DiscountDto>> GetDiscountsByFilter(DiscountFilterDto filter, CancellationToken cancellationToken = default);
        Task<DiscountDto> UpdateDiscount(DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteDiscount(int id, CancellationToken cancellationToken = default);
    }
}
