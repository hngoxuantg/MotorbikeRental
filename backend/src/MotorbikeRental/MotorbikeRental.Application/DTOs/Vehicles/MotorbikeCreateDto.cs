using Microsoft.AspNetCore.Http;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class MotorbikeCreateDto
    {
        public string MotorbikeName { get; set; }
        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid category ID")]
        public int CategoryId { get; set; } //FK tới Category table
        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(0.01, 9999999, ErrorMessage = "Hourly rate must be greater than 0")]
        public decimal HourlyRate { get; set; }//Thuê theo giờ

        [Required(ErrorMessage = "Daily rate is required")]
        [Range(0.01, 9999999, ErrorMessage = "Daily rate must be greater than 0")]
        public decimal DailyRate { get; set; }//Giá theo ngày
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(20, ErrorMessage = "Category name cannot exceed 20 characters")]
        public string LicensePlate { get; set; } // Biển số xe
        [Required(ErrorMessage = "Brand is required")]
        [MaxLength(20, ErrorMessage = "Brand cannot exceed 20 characters")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Year is required")]
        [Range(1990, 2030, ErrorMessage = "Year must be between 1990 and 2030")]
        public int Year { get; set; } //Năm sản xuất
        [MaxLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
        public string? Color { get; set; }
        [Required(ErrorMessage = "Engine capacity is required")]
        [Range(50, 2000, ErrorMessage = "Engine capacity must be between 50cc and 2000cc")]
        public int EngineCapacity { get; set; } //Dung tích động cơ
        [Required(ErrorMessage = "Chassis number is required")]
        [MaxLength(20, ErrorMessage = "Chassis number cannot exceed 20 characters")]
        public string ChassisNumber { get; set; } //Số khung 
        [Required(ErrorMessage = "Engine number is required")]
        [MaxLength(20, ErrorMessage = "Engine number cannot exceed 20 characters")]
        public string EngineNumber { get; set; } //Số máy
        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; } // Mô tả
        [Required(ErrorMessage = "Motorbike condition status is required")]
        public MotorbikeConditionStatus MotorbikeConditionStatus { get; set; } //Tình trạng xe(mới, cũ, ...)
        [Range(0, 999999, ErrorMessage = "Mileage must be positive")]
        public decimal? Mileage { get; set; } // Số km đã đi
        public IFormFile? FormFile { get; set; }
    }
}
