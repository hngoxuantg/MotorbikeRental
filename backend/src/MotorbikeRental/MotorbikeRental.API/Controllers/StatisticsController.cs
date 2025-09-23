using Microsoft.AspNetCore.Mvc;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Statistics;
using MotorbikeRental.Application.Interface.IServices.IStatisticsServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview(CancellationToken cancellationToken = default)
        {
            var result = await statisticsService.GetOverview(cancellationToken);

            var response = new ResponseDto<OverviewStatisticsDto>
            {
                Success = true,
                Message = "Overview statistics retrieved successfully.",
                Data = result
            };

            return Ok(response);
        }
        [HttpGet("motorbike-status")]
        public async Task<IActionResult> GetMotorbikeStatusStatistics(CancellationToken cancellationToken = default)
        {
            var result = await statisticsService.GetMotorbikeStatusStatistics(cancellationToken);

            var response = new ResponseDto<MotorbikeStatusStatisticsDto>
            {
                Success = true,
                Message = "Motorbike status statistics retrieved successfully.",
                Data = result
            };

            return Ok(response);
        }
        [HttpGet("contract-status")]
        public async Task<IActionResult> GetContractStatusStatistics(CancellationToken cancellationToken = default)
        {
            var result = await statisticsService.GetContractStatusStatistics(cancellationToken);

            var response = new ResponseDto<ContractStatusStatisticsDto>
            {
                Success = true,
                Message = "Contract status statistics retrieved successfully.",
                Data = result
            };

            return Ok(response);
        }
        [HttpGet("incident-statistics")]
        public async Task<IActionResult> GetIncidentStatistics(CancellationToken cancellationToken = default)
        {
            var result = await statisticsService.GetIncidentStatistics(cancellationToken);

            var response = new ResponseDto<IncidentStatisticsDto>
            {
                Success = true,
                Message = "Incident statistics retrieved successfully.",
                Data = result
            };

            return Ok(response);
        }
        [HttpGet("revenue-by-date-range")]
        public async Task<IActionResult> GetRevenueByDateRange([FromQuery] StatisticsFilterDto filter, CancellationToken cancellation = default)
        {
            var result = await statisticsService.GetRevenueByDateRange(filter, cancellation);

            var response = new ResponseDto<RevenueStatisticsDto>
            {
                Success = true,
                Message = "Revenue statistics by date range retrieved successfully.",
                Data = result
            };

            return Ok(response);
        }
        [HttpGet("monthly-highlights")]
        public async Task<IActionResult> GetMonthlyHighlight([FromQuery] HighlightFilterDto highlightFilterDto ,CancellationToken cancellationToken = default)
        {
            var result = await statisticsService.GetMonthlyHighlights(highlightFilterDto, cancellationToken);

            var response = new ResponseDto<MonthlyHighlightDto>
            {
                Success = true,
                Message = "Monthly highlights statistics retrieved successfully",
                Data = result
            };

            return Ok(response);
        }
    }
}
