using MotorbikeRental.Application.DTOs.Statistics;

namespace MotorbikeRental.Application.Interface.IServices.IStatisticsServices
{
    public interface IStatisticsService
    {
        Task<OverviewStatisticsDto> GetOverview(CancellationToken cancellationToken = default);
        Task<MotorbikeStatusStatisticsDto> GetMotorbikeStatusStatistics(CancellationToken cancellationToken = default);
        Task<ContractStatusStatisticsDto> GetContractStatusStatistics(CancellationToken cancellationToken = default);
        Task<IncidentStatisticsDto> GetIncidentStatistics(CancellationToken cancellationToken = default);
        Task<RevenueStatisticsDto> GetRevenueByDateRange(StatisticsFilterDto filter, CancellationToken cancellation = default);
        Task<MonthlyHighlightDto> GetMonthlyHighlights(HighlightFilterDto filter, CancellationToken cancellationToken = default);
    }
}
