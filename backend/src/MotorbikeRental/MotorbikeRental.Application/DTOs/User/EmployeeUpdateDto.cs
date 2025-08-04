using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeUpdateDto
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Salary must be positive")]
        public decimal? Salary { get; set; }
        [Required(ErrorMessage = "Employee status is required")]
        public EmployeeStatus Status { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}