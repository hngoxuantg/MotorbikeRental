namespace MotorbikeRental.Application.DTOs.MaintenanceRecord
{
    public class MaintenanceRecordFilterDto
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
        public bool? IsCompleted { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Search { get; set; }
    }
}