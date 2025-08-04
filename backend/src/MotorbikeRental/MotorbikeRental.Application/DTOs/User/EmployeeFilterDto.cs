using MotorbikeRental.Domain.Enums.UserEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeFilterDto
    {
        private int pageNumber = 1;
        private int pageSize = 12;
        [MaxLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? Search { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value >= 1) pageNumber = value;
            }
        }
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize
        {
            get => pageSize;
            set
            {
                if(value >= 1 && value <= 100) pageSize = value;
            }
        }
        public int? RoleId { get; set; }
        public EmployeeStatus? Status { get; set; } = null;
    }
}
