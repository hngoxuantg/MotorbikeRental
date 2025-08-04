using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage ="Category id is required")]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(20)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "DepositAmount is required")]
        [Range(1, double.MaxValue)]
        public double DepositAmount { get; set; }
    }
}
