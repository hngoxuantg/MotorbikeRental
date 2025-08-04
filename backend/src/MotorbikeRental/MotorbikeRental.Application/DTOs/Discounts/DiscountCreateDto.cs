using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Discount
{
    public class DiscountCreateDto
    {
        [Required(ErrorMessage = "Discount name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "At least one category ID is required")]
        public List<int> CategoryId { get; set; }
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Discount value is required")]
        [Range(1, 100, ErrorMessage = "Discount value must be between 1 and 100")]
        public int Value { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Created date is required")]
        public bool IsActive { get; set; }
    }
}
