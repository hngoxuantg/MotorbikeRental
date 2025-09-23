using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserCredentialsController : ControllerBase
    {
        private readonly IUserCredentialsService userCredentialsService;
        private readonly IMemoryCache memoryCache;
        public UserCredentialsController(IUserCredentialsService userCredentialsService, IMemoryCache memoryCache)
        {
            this.userCredentialsService = userCredentialsService;
            this.memoryCache = memoryCache;
        }
        [HttpPost("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateUserCredentials(int id, [FromBody] UserCredentialsCreateDto userCredentialsCreateDto, CancellationToken cancellationToken = default)
        {
            if (id != userCredentialsCreateDto.EmployeeId)
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });

            var result = await userCredentialsService.CreateUserCredentials(userCredentialsCreateDto, cancellationToken);

            var response = new ResponseDto
            {
                Success = true,
                Message = "User credentials created successfully",
            };

            return CreatedAtAction(nameof(GetUserCredentials), new { id }, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserCredentials(int id, CancellationToken cancellation = default)
        {
            var result = new UserCredentialsDto();

            if (memoryCache.TryGetValue($"UserCredentials_{id}", out UserCredentialsDto? userCredentials))
            {
                result = userCredentials;
            }
            else
            {
                result = await userCredentialsService.GetUserCredentialsByEmployeeId(id, cancellation);
                if (result != null)
                    memoryCache.Set($"UserCredentials_{id}", result, TimeSpan.FromMinutes(10));
            }

            var response = new ResponseDto<UserCredentialsDto>
            {
                Success = true,
                Message = "User credentials retrieved successfully",
                Data = result
            };

            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateUserCredentials(int id, [FromBody] UserCredentialsUpdateDto userCredentialsUpdateDto, CancellationToken cancellationToken = default)
        {
            if (id != userCredentialsUpdateDto.EmployeeId)
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });

            await userCredentialsService.UpdateUserCredentialsByAdmin(userCredentialsUpdateDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");

            var response = new ResponseDto
            {
                Success = true,
                Message = "User credentials update successfully"
            };

            return Ok(response);
        }
        [HttpPost("{id}/reset-email")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ResetEmail(int id, [FromBody] ResetEmailDto resetEmailDto, CancellationToken cancellationToken = default)
        {
            if (resetEmailDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }

            var result = await userCredentialsService.ResetEmail(resetEmailDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");
            memoryCache.Remove($"Employee_{id}");

            var response = new ResponseDto
            {
                Success = true,
                Message = "Email reset successfully",
            };

            return Ok(response);
        }
        [HttpPost("{id}/reset-password")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ResetPasswordByAdmin(int id, [FromBody] ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
        {
            if (resetPasswordDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }

            var result = await userCredentialsService.ResetPasswordByAdmin(resetPasswordDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");
            memoryCache.Remove($"Employee_{id}"); 

            var response = new ResponseDto
            {
                Success = true,
                Message = "Password reset successfully",
            };

            return Ok(response);
        }
        [HttpPost("{id}/reset-phone-number")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ResetPhoneNumber(int id, [FromBody] ResetPhoneNumberDto resetPhoneNumberDto, CancellationToken cancellationToken = default)
        {
            if (resetPhoneNumberDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }

            var result = await userCredentialsService.ResetPhoneNumber(resetPhoneNumberDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");

            var response = new ResponseDto
            {
                Success = true,
                Message = "Phone number reset successfully",
            };

            return Ok(response);
        }
        [HttpPost("{id}/reset-user-name")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ResetUserName(int id, [FromBody] ResetUserNameDto resetUserNameDto, CancellationToken cancellationToken = default)
        {
            if (resetUserNameDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }

            var result = await userCredentialsService.ResetUserName(resetUserNameDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");
            memoryCache.Remove($"Employee_{id}"); 

            var response = new ResponseDto
            {
                Success = true,
                Message = "Username reset successfully",
            };

            return Ok(response);
        }
        [HttpPost("{id}/reset-role")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ResetRole(int id, [FromBody] ResetRoleDto resetRoleDto, CancellationToken cancellationToken = default)
        {
            if (resetRoleDto.EmployeeId != id)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Employee ID in the request body does not match the ID in the URL."
                });
            }

            var result = await userCredentialsService.ResetRoleByAdmin(resetRoleDto, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");
            memoryCache.Remove($"Employee_{id}"); 

            var response = new ResponseDto
            {
                Success = true,
                Message = "Role reset successfully",
            };

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUserCredentials(int id, CancellationToken cancellationToken = default)
        {
            await userCredentialsService.DeleteUserCredentialsByAdmin(id, cancellationToken);

            memoryCache.Remove($"UserCredentials_{id}");
            memoryCache.Remove($"Employee_{id}"); 

            var response = new ResponseDto
            {
                Success = true,
                Message = "User credentials deleted successfully",
            };

            return Ok(response);
        }
    }
}
