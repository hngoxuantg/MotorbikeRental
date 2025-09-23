using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.IMailServices;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using System.Security.Claims;
using MotorbikeRental.Application.DTOs.Emails;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.UserServices
{
    public class UserCredentialsService : IUserCredentialsService
    {
        private readonly IUserCredentialsValidator userCredentialsValidator;
        private readonly IMapper mapper;
        private readonly UserManager<UserCredentials> userManager;
        private readonly RoleManager<Roles> roleManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMailService mailService;
        private readonly IUnitOfWork unitOfWork;
        public UserCredentialsService(IUserCredentialsValidator userCredentialsValidator, IMapper mapper, UserManager<UserCredentials> userManager, RoleManager<Roles> roleManager, IHttpContextAccessor httpContextAccessor, IMailService mailService, IUnitOfWork unitOfWork)
        {
            this.userCredentialsValidator = userCredentialsValidator;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
            this.mailService = mailService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateUserCredentials(UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default)
        {
            await userCredentialsValidator.ValidatorForCreate(userCredentialsCreateDto, cancellationToken);

            Roles? roles = await roleManager.FindByIdAsync(userCredentialsCreateDto.RoleId.ToString());

            if (roles == null)
                throw new NotFoundException($"Role with id {userCredentialsCreateDto.RoleId} not found");

            UserCredentials userCredentials = mapper.Map<UserCredentials>(userCredentialsCreateDto);

            if (userCredentialsCreateDto.UserName == null)
                userCredentials.UserName = userCredentialsCreateDto.Email;

            IdentityResult identityResult = await userManager.CreateAsync(userCredentials, userCredentialsCreateDto.Password);

            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(userCredentials, roles.Name);

            await mailService.SendEmail(new EmailDto
            {
                To = userCredentials.Email,
                UserName = userCredentials.UserName,
                Password = userCredentialsCreateDto.Password,
            });

            return true;
        }
        public async Task<UserCredentialsDto> GetUserCredentialsByEmployeeId(int employeeId, CancellationToken cancellationToken = default)
        {
            UserCredentials? userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(employeeId, true, cancellationToken);

            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with employeeId {employeeId} not found");

            return mapper.Map<UserCredentialsDto>(userCredentials);
        }
        public async Task<bool> UpdateUserCredentialsByAdmin(UserCredentialsUpdateDto userCredentialUpdateDto, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            await userCredentialsValidator.ValidatorForUpdate(userCredentialUpdateDto, cancellationToken);

            Roles? roles = await roleManager.FindByIdAsync(userCredentialUpdateDto.RoleId.ToString());

            if (roles == null)
                throw new NotFoundException($"Role with id {userCredentialUpdateDto.RoleId} not found");

            UserCredentials userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(userCredentialUpdateDto.EmployeeId, true, cancellationToken);

            if (userCredentialUpdateDto.Email != userCredentials.Email)
            {
                IdentityResult identityResult = await userManager.SetEmailAsync(userCredentials, userCredentialUpdateDto.Email);

                if (!identityResult.Succeeded)
                    errors.Add(string.Join("; ", identityResult.Errors.Select(e => e.Description)));
            }
            if (userCredentialUpdateDto.PhoneNumber != userCredentials.PhoneNumber)
            {
                IdentityResult identityResult = await userManager.SetPhoneNumberAsync(userCredentials, userCredentialUpdateDto.PhoneNumber);

                if (!identityResult.Succeeded)
                    errors.Add(string.Join("; ", identityResult.Errors.Select(e => e.Description)));
            }
            if (userCredentialUpdateDto.UserName != userCredentials.UserName)
            {
                IdentityResult identityResult = await userManager.SetUserNameAsync(userCredentials, userCredentialUpdateDto.UserName);

                if (!identityResult.Succeeded)
                    errors.Add(string.Join("; ", identityResult.Errors.Select(e => e.Description)));
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(userCredentials);

            IdentityResult identity = await userManager.ResetPasswordAsync(userCredentials, token, userCredentialUpdateDto.Password);

            if (!identity.Succeeded)
                errors.Add(string.Join("; ", identity.Errors.Select(e => e.Description)));
            if (errors.Any())
                throw new ValidatorException(string.Join("; ", errors));

            return true;
        }
        public async Task<bool> ResetEmail(ResetEmailDto resetEmail, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(resetEmail.EmployeeId, true, cancellationToken);

            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetEmail.EmployeeId} not found");
            if (userCredentials.Email == resetEmail.Email)
                throw new ValidatorException("New email cannot be the same as the current email");
            if (userCredentials.Email == userCredentials.UserName)
                await userManager.SetUserNameAsync(userCredentials, resetEmail.Email);

            IdentityResult identityResult = await userManager.SetEmailAsync(userCredentials, resetEmail.Email);

            return identityResult.Succeeded ? true : throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
        public async Task<bool> ResetPhoneNumber(ResetPhoneNumberDto resetPhoneNumber, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(resetPhoneNumber.EmployeeId, true, cancellationToken);

            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetPhoneNumber.EmployeeId} not found");
            if (userCredentials.PhoneNumber == resetPhoneNumber.PhoneNumber)
                throw new ValidatorException("New phone number cannot be the same as the current phone number");
            IdentityResult identityResult = await userManager.SetPhoneNumberAsync(userCredentials, resetPhoneNumber.PhoneNumber);

            return identityResult.Succeeded ? true : throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
        public async Task<bool> ResetUserName(ResetUserNameDto resetUserName, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(resetUserName.EmployeeId, true, cancellationToken);

            if (userCredentials.UserName == resetUserName.UserName)
                throw new ValidatorException("New username cannot be the same as the current username");
            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetUserName.EmployeeId} not found");
            if (userCredentials.UserName == resetUserName.UserName)
                throw new ValidatorException("New username cannot be the same as the current username");

            IdentityResult identityResult = await userManager.SetUserNameAsync(userCredentials, resetUserName.UserName);

            return identityResult.Succeeded ? true : throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
        public async Task<bool> ResetPasswordByAdmin(ResetPasswordDto resetPassword, CancellationToken cancellationToken = default)
        {
            UserCredentials userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(resetPassword.EmployeeId, true, cancellationToken);

            if (userCredentials == null)
                throw new NotFoundException($"UserCredentials with id {resetPassword.EmployeeId} not found");

            string token = await userManager.GeneratePasswordResetTokenAsync(userCredentials);

            IdentityResult identityResult = await userManager.ResetPasswordAsync(userCredentials, token, resetPassword.Password);

            return identityResult.Succeeded ? true : throw new ValidatorException(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }
        public async Task<bool> ResetRoleByAdmin(ResetRoleDto resetRole, CancellationToken cancellationToken = default)
        {
            string? employeeId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (employeeId == null || int.Parse(employeeId) == resetRole.EmployeeId)
                throw new NotFoundException("You cannot reset your own role, please contact the administrator to reset your role.");

            UserCredentials? userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(resetRole.EmployeeId, true, cancellationToken);

            if (userCredentials == null)
                throw new NotFoundException($"UserCredential with id {resetRole.EmployeeId} not found");

            Roles? role = await roleManager.FindByIdAsync(resetRole.RoleId.ToString());

            if (role == null)
                throw new NotFoundException($"Role with id {resetRole.RoleId} not found");

            userCredentials.RoleId = resetRole.RoleId;

            IdentityResult identityResult = await userManager.UpdateAsync(userCredentials);

            if (!identityResult.Succeeded)
                throw new Exception(string.Join("; ", identityResult.Errors.Select(e => e.Description)));

            IList<string>? roles = await userManager.GetRolesAsync(userCredentials);

            await userManager.RemoveFromRolesAsync(userCredentials, roles);
            await userManager.AddToRoleAsync(userCredentials, role.Name);

            return true;
        }
        public async Task<bool> DeleteUserCredentialsByAdmin(int employeeId, CancellationToken cancellationToken = default)
        {
            UserCredentials? userCredentials = await unitOfWork.UserCredentialsRepository.GetByEmployeeId(employeeId, true, cancellationToken);

            string? id = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await userCredentialsValidator.ValidatorForDelete(id != null ? int.Parse(id) : null, userCredentials);

            IdentityResult identityResult = await userManager.DeleteAsync(userCredentials);

            if (!identityResult.Succeeded)
                throw new ValidatorException(string.Join("; ", identityResult.Errors.Select(e => e.Description)));

            return true;
        }
    }
}