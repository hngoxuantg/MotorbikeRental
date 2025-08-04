using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.AuthenticDto
{
    public class UserCredentialsDto
    {
        public string EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleName { get; set; }
    }
}
