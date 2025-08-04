using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class MotorbikeDto
    {
        public int MotorbikeId { get; set; }
        public string MotorbikeName { get; set; }
        public int CategoryId { get; set; } //FK tới Category table
        public string? CategoryName { get; set; }
        public decimal HourlyRate { get; set; }//Thuê theo giờ
        public decimal DailyRate { get; set; }//Giá theo ngày
        public string LicensePlate { get; set; } // Biển số xe
        public string Brand { get; set; }
        public int Year { get; set; } //Năm sản xuất
        public string? Color { get; set; }
        public int EngineCapacity { get; set; } //Dung tích động cơ
        public string ChassisNumber { get; set; } //Số khung 
        public string EngineNumber { get; set; } //Số máy
        public string? Description { get; set; } // Mô tả
        public MotorbikeConditionStatus MotorbikeConditionStatus { get; set; } //Tình trạng xe(mới, cũ, ...)
        public string? ImageUrl { get; set; }
        public decimal? Mileage { get; set; } // Số km đã đi
        public MotorbikeStatus Status { get; set; } //Thông tin xe
    }
}