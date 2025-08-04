namespace MotorbikeRental.Application.DTOs.Statistics
{
    public class IncidentStatisticsDto
    {
        public int TotalIncidents { get; set; } //Tổng số sự cố
        public int ResolvedIncidents { get; set; } //Số sự cố đã giải quyết
        public int UnresolvedIncidents { get; set; } //Số sự cố chưa giải quyết
    }
}
