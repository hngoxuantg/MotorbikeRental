using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; } // Mã bảo trì
        public int MotorbikeId { get; set; } // Mã xe
        public string MotorbikeName { get; set; } // Tên xe
        public string MotorbikeImage { get; set; } // Hình ảnh xe
        public DateTime? MaintenanceDate { get; set; } // Ngày bảo trì
        public string Description { get; set; } // Mô tả bảo trì
        public decimal Cost { get; set; } // Chi phí bảo trì
        public bool IsCompleted { get; set; } // Trạng thái hoàn thành bảo trì
        public int EmployeeId { get; set; } // Mã nhân viên thực hiện bảo trì
        public string EmployeeName { get; set; }
        public DateTime CreatedAt { get; set; } // Ngày tạo bản ghi
    }
}
