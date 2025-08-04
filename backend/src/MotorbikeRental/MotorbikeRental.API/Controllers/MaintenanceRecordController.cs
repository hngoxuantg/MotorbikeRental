using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class MaintenanceRecordController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IMaintenanceRecordService maintenanceRecordService;

        public MaintenanceRecordController(IMemoryCache memoryCache, IMaintenanceRecordService maintenanceRecordService)
        {
            this.memoryCache = memoryCache;
            this.maintenanceRecordService = maintenanceRecordService;
        }

        [Authorize(Roles = "Maintenance")]
        [HttpPost]
        public async Task<IActionResult> CreateMaintenanceRecord(
            [FromBody] MaintenanceRecordCreateDto maintenanceRecordCreateDto,
            CancellationToken cancellationToken = default)
        {
            await maintenanceRecordService.CreateMaintenanceRecord(maintenanceRecordCreateDto, cancellationToken);
            var response = new ResponseDto
            {
                Success = true,
                Message = "Maintenance record created successfully",
            };
            return Ok(response);
        }

        [Authorize(Roles = "Maintenance")]
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteMaintenanceRecord(
            [FromBody] MaintenanceCompletionDto maintenanceRecordCompleteDto,
            CancellationToken cancellationToken = default)
        {
            var result =
                await maintenanceRecordService.MaintenanceRecordComplete(maintenanceRecordCompleteDto,
                    cancellationToken);
            memoryCache.Remove($"MaintenanceRecord_{result.MaintenanceRecordId}");
            var response = new ResponseDto<MaintenanceRecordDto>
            {
                Success = true,
                Message = "Maintenance record completed successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Maintenance")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaintenanceRecordById(int id, CancellationToken cancellationToken = default)
        {
            var result = new MaintenanceRecordDto();
            if (memoryCache.TryGetValue($"MaintenanceRecord_{id}", out MaintenanceRecordDto? maintenanceRecordDto))
            {
                result = maintenanceRecordDto;
            }
            else
            {
                result = await maintenanceRecordService.GetById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"MaintenanceRecord_{id}", result, TimeSpan.FromMinutes(10));
            }

            var response = new ResponseDto<MaintenanceRecordDto>
            {
                Success = true,
                Message = "Maintenance record retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Maintenance")]
        [HttpGet]
        public async Task<IActionResult> GetMaintenanceRecordByFilter([FromQuery] MaintenanceRecordFilterDto filterDto,
            CancellationToken cancellationToken = default)
        {
            var result = await maintenanceRecordService.GetMaintenanceRecordByFilter(filterDto, cancellationToken);
            var response = new ResponseDto<PaginatedDataDto<MaintenanceRecordDto>>
            {
                Success = true,
                Message = "Maintenance records retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Maintenance")]
        [HttpGet("motorbike/{motorbikeId}")]
        public async Task<IActionResult> GetMotorbikeMaintenanceInfo(int motorbikeId,
            CancellationToken cancellationToken = default)
        {
            var result =
                await maintenanceRecordService.GetMotorbikeMaintenanceInfoByMotorbikeId(motorbikeId, cancellationToken);
            var response = new ResponseDto<MaintenanceMotorbikeDto>
            {
                Success = true,
                Message = "Motorbike maintenance info retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Maintenance")]
        [HttpGet("motorbikes")]
        public async Task<IActionResult> GetMotorbikesWithMaintenanceInfoByFilter(
            [FromQuery] MaintenanceMotorbikeFilterDto filter, CancellationToken cancellationToken = default)
        {
            var result =
                await maintenanceRecordService.GetMotorbikesWithMaintenanceInfoByFilter(filter, cancellationToken);
            var response = new ResponseDto<PaginatedDataDto<MaintenanceMotorbikeDto>>
            {
                Success = true,
                Message = "Motorbikes with maintenance info retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}