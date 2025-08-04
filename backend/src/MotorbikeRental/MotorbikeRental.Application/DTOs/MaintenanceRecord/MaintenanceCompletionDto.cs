using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceCompletionDto
    {
        [Required(ErrorMessage = "Motorbike ID is required.")]
        public int MotorbikeId { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } // Mô tả bảo trì
        [Required(ErrorMessage = "Cost is required.")]
        [Range(0, 999999999)]
        public decimal Cost { get; set; } // Chi phí bảo trì
        [Required(ErrorMessage = "NextMaintenanceDate is required.")]
        public DateTime NextMaintenanceDate { get; set; } // Ngày bảo trì tiếp theo
    }
}
