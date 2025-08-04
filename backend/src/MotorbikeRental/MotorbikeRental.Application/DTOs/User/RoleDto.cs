using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.User
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public string? Description { get; set; }
    }
}