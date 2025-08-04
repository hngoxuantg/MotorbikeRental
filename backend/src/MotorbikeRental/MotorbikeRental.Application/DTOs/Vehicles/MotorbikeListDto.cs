using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class MotorbikeListDto
    {
        public int MotorbikeId { get; set; }
        public string MotorbikeName { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }
        public string? ImageUrl { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public MotorbikeStatus Status { get; set; } //Thông tin xe
    }
}