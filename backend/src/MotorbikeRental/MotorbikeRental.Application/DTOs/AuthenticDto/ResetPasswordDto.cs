using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.AuthenticDto
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6–50 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetEmailDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
    public class ResetPhoneNumberDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(13, ErrorMessage = "Phone number cannot exceed 13 characters")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
    }
    public class ResetUserNameDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        public string UserName { get; set; }
    }
    public class ResetRoleDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "RoleId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RoleId must be a positive integer")]
        public int RoleId { get; set; }
    }
}
