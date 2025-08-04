using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceMotorbikeFilterDto
    {
        private int pageNumber = 1;
        private int pageSize = 12;
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value >= 1) pageNumber = value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value >= 1 && value <= 100) pageSize = value;
            }
        }
        public string? Search { get; set; }
        public MotorbikeStatus? Status { get; set; } // Trạng thái của xe máy
    }
}
