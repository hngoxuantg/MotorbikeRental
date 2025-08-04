using MotorbikeRental.Application.DTOs.AuthenticDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Interface.IServices.IAuthServices
{
    public interface IAuthService
    {
        Task<UserCredentialsDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default);
    }
}
