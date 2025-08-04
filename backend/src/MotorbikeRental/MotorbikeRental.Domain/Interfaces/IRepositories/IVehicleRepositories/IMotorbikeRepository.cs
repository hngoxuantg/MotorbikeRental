using System.Globalization;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories
{
    public interface IMotorbikeRepository : IBaseRepository<Motorbike>
    {
        Task<Motorbike> GetByIdNoAsTracking(int id, CancellationToken cancellationToken = default);
        Task<Motorbike> GetByIdWithIncludes(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Motorbike>, int totalCount)> GetFilterData(int? categoryId, string? brand, string? search, MotorbikeStatus? status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetDistinctBrands(CancellationToken cancellationToken = default);
        Task<Motorbike?> GetMotorbikeBasicInfoById(int motorbikeId, CancellationToken cancellationToken = default);
    }
}