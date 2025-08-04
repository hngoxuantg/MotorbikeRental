namespace MotorbikeRental.Application.DTOs.Payments
{
    public class PaymentFilterDto
    {
        private int pageNumber { get; set; } = 1;
        private int pageSize { get; set; } = 12;
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
        public DateTime? StarDate { get; set; } // Ngày bắt đầu
        public string? Search { get; set; } // Từ khóa tìm kiếm
    }
}
