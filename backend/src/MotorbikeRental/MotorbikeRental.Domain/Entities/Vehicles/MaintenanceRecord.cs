using MotorbikeRental.Domain.Entities.User;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MotorbikeId { get; set; } //FK tới MotorBike Table
        public virtual Motorbike Motorbike { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; }   // Ngày bảo dưỡng

        [StringLength(500)]
        public string? Description { get; set; } //Mô tả công việc

        [Range(0, 999999999)]
        public decimal? Cost { get; set; } //Chi phí bảo dương(sửa chữa)

        [Required]
        public bool IsCompleted { get; set; } = false;
        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; } // FK Nhân viên bảo dưỡng
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } //Ngày tạo bảng ghi
    }
}