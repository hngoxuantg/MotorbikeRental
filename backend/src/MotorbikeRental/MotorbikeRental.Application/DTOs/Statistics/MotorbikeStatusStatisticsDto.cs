using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class MotorbikeStatusStatisticsDto
    {
        public int Available { get; set; } //Xe sẵn
        public int Rented { get; set; } //Xe đang thuê
        public int UnderMaintenance { get; set; } //Xe đang bảo trì
        public int Reserved { get; set; } //Xe đã đặt trước
        public int OutOfService { get; set; } //Xe không hoạt động
        public int Damaged { get; set; } //Xe hư hỏng
    }
}
        