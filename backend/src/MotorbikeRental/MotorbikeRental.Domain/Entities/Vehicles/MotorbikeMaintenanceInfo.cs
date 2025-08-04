using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class MotorbikeMaintenanceInfo
    {
        [Required]
        public int MotorbikeId { get; set; } // Mã xe
        public DateTime? LastMaintenanceDate { get;set; } // Ngày bảo trì gần nhất
        public DateTime? NextMaintenanceDate { get; set; } // Ngày bảo trì tiếp theo
        public int MaintenanceCount { get; set; } // Số lần bảo trì đã thực hiện
        public virtual Motorbike Motorbike { get; set; } 
    }
}
