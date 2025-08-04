using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceRecordCreateDto
    {
        [Required(ErrorMessage = "MotorbikeId is required.")]
        public int MotorbikeId { get; set; } // Mã xe
        [Required(ErrorMessage = "MaintenanceDate is required.")]
        public DateTime MaintenanceDate { get; set; } // Ngày bảo trì
        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; } // Mã nhân viên thực hiện bảo trì
    }
}
