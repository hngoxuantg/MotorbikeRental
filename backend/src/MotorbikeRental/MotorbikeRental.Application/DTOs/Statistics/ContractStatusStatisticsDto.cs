namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class ContractStatusStatisticsDto
    {
        public int TotalContracts { get; set; } //Tổng hợp đồng
        public int TotalActiveContracts { get; set; } //Tổng hợp đồng đang hoạt động
        public int TotalCompletedContracts { get; set; } //Tổng hợp đồng đã hoàn thành
        public int TotalCancelledContracts { get; set; } //Tổng hợp đồng đã hủy
        public int TotalPendingContracts { get; set; } //Tổng hợp đồng đang chờ xử lý
        public int TotalProcessingContracts { get; set; } // Tổng hợp đồng đang xử lý sự cố
    }
}
