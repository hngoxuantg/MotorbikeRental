using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Domain.Entities.User;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles(CancellationToken cancellationToken = default);
        Task<RoleDto> GetRoleById(int id, CancellationToken cancellationToken = default);
    }
}