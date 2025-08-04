using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserCredentials> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public AuthService(UserManager<UserCredentials> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<UserCredentialsDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            UserCredentials? userCredentials = await unitOfWork.UserCredentialsRepository.GetByUserNameInCludes(loginDto.UserName, cancellationToken) ?? throw new NotFoundException($"UserCredentials with username {loginDto.UserName} not found");
            if (userCredentials?.Employee.Status != 0)
                throw new NotFoundException($"Employee with id {userCredentials?.EmployeeId} is not active");
            if (await userManager.CheckPasswordAsync(userCredentials, loginDto.Password))
            {
                userCredentials.LastLogin = DateTime.UtcNow;
                await unitOfWork.UserCredentialsRepository.Update(userCredentials, cancellationToken);
                return mapper.Map<UserCredentialsDto>(userCredentials);
            }
            return null;
        }
    }
}
