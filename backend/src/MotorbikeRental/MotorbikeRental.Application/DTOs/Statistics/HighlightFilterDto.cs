using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class HighlightFilterDto
    {
        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set;  }
        [Required(ErrorMessage = "Year is required.")]
        public int Year { get; set; }
    }
}
