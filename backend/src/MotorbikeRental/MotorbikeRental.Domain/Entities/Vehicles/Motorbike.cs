using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class Motorbike
    {
        public int MotorbikeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string MotorbikeName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; } //FK tới Category table
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(15)]
        public string LicensePlate { get; set; } // Biển số xe

        [Required]
        [MaxLength(20)]
        public string Brand { get; set; }

        [Required]
        [Range(1990, 2030)]
        public int Year { get; set; } //Năm sản xuất

        [MaxLength(20)]
        public string? Color { get; set; }

        [Required]
        [Range(50, 2000)]
        public int EngineCapacity { get; set; } //Dung tích động cơ

        [Required]
        [MaxLength(20)]
        public string ChassisNumber { get; set; } //Số khung 

        [Required]
        [MaxLength(20)]
        public string EngineNumber { get; set; } //Số máy

        [MaxLength(100)]
        public string? Description { get; set; } // Mô tả

        [Required]
        public MotorbikeConditionStatus MotorbikeConditionStatus { get; set; } //Tình trạng xe(mới, cũ, ...)

        [Url]
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, 999999)]
        public decimal? Mileage { get; set; } // Số km đã đi

        [Required]
        public MotorbikeStatus Status { get; set; } //Thông tin xe

        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } //Hồ sơ bảo trì
        public virtual ICollection<RentalContract> RentalContracts { get; set; } //Hơp đồng thuê xe với khách
        public virtual ICollection<Incident> Incidents { get; set; }  //Sự cố
        public virtual MotorbikeMaintenanceInfo MotorbikeMaintenanceInfo { get; set; } //Thông tin bảo trì xe
        public virtual PriceList PriceList { get; set; } //Bảng giá thuê xe
    }
}