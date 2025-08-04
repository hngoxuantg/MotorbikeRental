namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class RevenueStatisticsDto
    {
        public DateTime? ToDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalRevenue { get; set; } //Tổng doanh thu
        public decimal TotalMaintenanceCost { get; set; } //Tổng chi phí bảo trì 
        public int TotalContracts { get; set; } //Tổng số hợp đồng 
        public IEnumerable<DailyRevenueDto>? DailyRevenues { get; set; }
        public IEnumerable<MonthlyRevenueDto>? MonthlyRevenues { get; set; }
        public IEnumerable<YearlyRevenueDto>? YearlyRevenues { get; set; }
    }
    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
    }
    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Monthly { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
    }
    public class YearlyRevenueDto
    {
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
    }
}
