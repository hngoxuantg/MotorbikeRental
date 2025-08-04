using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.User;

namespace MotorbikeRental.Application.Interface.IServices.IUserServices
{
    public interface IUserCredentialsService
    {
        Task<bool> CreateUserCredentials(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateUserCredentialsByAdmin(UserCredentialsUpdateDto userCredentialUpdateDto, CancellationToken cancellationToken = default);
        Task<bool> ResetEmail(ResetEmailDto resetEmail, CancellationToken cancellationToken = default);
        Task<bool> ResetUserName(ResetUserNameDto resetUserName, CancellationToken cancellationToken = default);
        Task<bool> ResetPasswordByAdmin(ResetPasswordDto resetPassword, CancellationToken cancellationToken = default);
        Task<bool> ResetPhoneNumber(ResetPhoneNumberDto resetPhoneNumber, CancellationToken cancellationToken = default);
        Task<bool> ResetRoleByAdmin(ResetRoleDto resetRole, CancellationToken cancellationToken = default);
        Task<bool> DeleteUserCredentialsByAdmin(int employeeId, CancellationToken cancellationToken = default);
        Task<UserCredentialsDto> GetUserCredentialsByEmployeeId(int employeeId, CancellationToken cancellationToken = default);
    }
}