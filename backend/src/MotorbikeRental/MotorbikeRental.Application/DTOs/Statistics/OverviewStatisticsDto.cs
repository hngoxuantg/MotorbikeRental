namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class OverviewStatisticsDto
    {
        public int TotalMotorbikes { get; set; } //Tổng xe
        public int TotalRentals { get; set; } //Tổng hợp đồng thuê
        public int TotalCustomers { get; set; } //Tổng khách hàng
        public decimal TotalRevenue { get; set; } //Tổng doanh thu
        public decimal TotalSalary { get; set; } //Tổng lương
        public decimal TotalMaintenanceCost { get; set; } //Tổng chi phí bảo trì
        public decimal Profit //Lợi nhận 
        {
            get
            {
                return TotalRevenue - TotalMaintenanceCost - TotalSalary;
            }
        }
        public int TotalIncidents { get; set; } //Tổng sự cố
        public int TotalEmployees { get; set; } //Tổng nhân viên
        public DateTime SystemStartDate { get; set; } //Ngày bắt đầu hệ thống
        public int TotalActiveDays // Tổng số ngày hoạt động của hệ thống
        {
            get
            {
                return (DateTime.UtcNow - SystemStartDate).Days;
            }
        }
    }
}
