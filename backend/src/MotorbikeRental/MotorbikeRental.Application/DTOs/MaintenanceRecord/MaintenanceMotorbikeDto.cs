using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceMotorbikeDto
    {
        public int MotorbikeId { get; set; }
        public string MotorbikeName { get; set; } 
        public string LicensePlate { get; set; } 
        public MotorbikeStatus MotorbikeStatus { get; set; } // Trạng thái xe máy
        public string ImageUrl { get; set; } 
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int EngineCapacity { get; set; } // Dung tích động cơ
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public int DaysUntilNextMaintenance { get; set; } // Số ngày còn lại đến
        public int MaintenanceCount { get; set; } // Số lần bảo trì
    }
}
