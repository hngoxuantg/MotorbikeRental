using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotorbikeRental.Common.Options;
using MotorbikeRental.Application.DTOs.AuthenticDto;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorbikeRental.Application.Services.AuthServices
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly AppSettings appSettings;
        public JwtTokenService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }
        public string GenerateJwtToken(UserCredentialsDto userCredentialsDto)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.JwtConfig.Secret);
            
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Aud, appSettings.JwtConfig.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iss, appSettings.JwtConfig.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Sub, userCredentialsDto.EmployeeId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, userCredentialsDto.FullName),
                new Claim(ClaimTypes.Role, userCredentialsDto.RoleName)
            };
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(appSettings.JwtConfig.TokenExpirationMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = jwtTokenHandler.CreateToken(securityTokenDescriptor);
            string jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}


