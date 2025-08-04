namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class MonthlyHighlightDto
    {
        public int Month { get; set; } 
        public int Year { get; set; }
        public IEnumerable<TopEmployeeOfMonthDto> TopEmployees { get; set; }
        public IEnumerable<TopRentedMotorbikeDto> TopMotorbikes { get; set; }
    }

    public class TopEmployeeOfMonthDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string? Avatar { get; set; }
        public int ContractCount { get; set; }
    }

    public class TopRentedMotorbikeDto
    {
        public int MotorbikeId { get; set; }
        public string MotorbikeName { get; set; }
        public string? MotorbikeImage { get; set; }
        public int RentalCount { get; set; }
    }
}
    