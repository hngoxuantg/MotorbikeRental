using Microsoft.AspNetCore.Http;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Vehicles;

namespace MotorbikeRental.Application.Interface.IServices.IVehicleServices
{
    public interface IMotorbikeService
    {
        Task<MotorbikeDto> CreateMotorbike(MotorbikeCreateDto motorbikeDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteMotorbike(int categoryId, CancellationToken cancellationToken = default);
        Task<MotorbikeDto> UpdateMotorbike(MotorbikeUpdateDto motorbikeDto, CancellationToken cancellationToken = default);
        Task<MotorbikeDto> GetMotorbikeById(int id, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<MotorbikeListDto>> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto, CancellationToken cancellationToken = default);
        Task<MotorbikeIndexDto> GetMotorbikeFilterOptions(CancellationToken cancellationToken = default);
    }
}