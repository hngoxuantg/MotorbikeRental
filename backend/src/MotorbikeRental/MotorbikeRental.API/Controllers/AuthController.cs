using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IJwtTokenService jwtTokenService;
        public AuthController(IAuthService authService, IJwtTokenService jwtTokenService)
        {
            this.authService = authService;
            this.jwtTokenService = jwtTokenService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var result = await authService.Login(loginDto, cancellationToken);
            if (result == null)
            {
                return Unauthorized(new ResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }
            string token = jwtTokenService.GenerateJwtToken(result);
            var response = new AuthResultDto
            {
                Success = true,
                Message = "Login successful",
                AccessToken = token,
            };
            return Ok(response);
        }
    }
}
