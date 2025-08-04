using MotorbikeRental.Domain.Enums.IncidentEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentFilterDto
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
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MotorbikeStatus? MotorbikeStatus { get; set; }
        public bool? IsResolved { get; set; }
    }
}