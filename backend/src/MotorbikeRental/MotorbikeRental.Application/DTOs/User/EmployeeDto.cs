using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string? Avatar { get; set; }
        public string? RoleName { get; set; } = "Account not created yet";
        public DateTime StartDate { get; set; }
        public decimal? Salary { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}