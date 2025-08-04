using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "CategoryName is required.")]
        [MaxLength(20, ErrorMessage = "CategoryName cannot exceed 20 characters.")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "DepositAmount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "DepositAmount must be a positive number.")]
        public decimal DepositAmount { get; set; }
    }
}
