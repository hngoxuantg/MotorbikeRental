using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService incidentService;
        private readonly IMemoryCache memoryCache;
        public IncidentController(IIncidentService incidentService, IMemoryCache memoryCache)
        {
            this.incidentService = incidentService;
            this.memoryCache = memoryCache;
        }
        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> CreateIncident([FromForm] IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.CreateIncident(incidentCreateDto, cancellationToken);
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident create successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Receptionist")]
        [HttpPut("before-complete")]
        public async Task<IActionResult> UpdateBeforeComplete([FromForm] IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.UpdateBeforeComplete(incidentUpdateBeforeCompleteDto, cancellationToken);
            memoryCache.Remove($"Incident_{result.IncidentId}");
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident updated successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("{id}/by-id")]
        public async Task<IActionResult> GetIncidentById(int id, CancellationToken cancellationToken = default)
        {
            var result = new IncidentDto();
            if (memoryCache.TryGetValue($"Incident_{id}", out IncidentDto? incidentDto))
            {
                result = incidentDto;
            }
            else
            {
                result = await incidentService.GetIncidentById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Incident_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpGet("{id}/by-contractId")]
        public async Task<IActionResult> GetIncidentByContractId(int id, CancellationToken cancellationToken = default)
        {
            var result = new IncidentDto();
            if(memoryCache.TryGetValue($"Incident_Contract_{id}", out IncidentDto? incidentDto))
            {
                result = incidentDto;
            }
            else
            {
                result = await incidentService.GetIncidentByContractId(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Incident_Contract_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident by contract ID retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles ="Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id, CancellationToken cancellationToken = default)
        {
            bool result = await incidentService.DeleteIncident(id, cancellationToken);
            memoryCache.Remove($"Incident_{id}");
            var response = new ResponseDto
            {
                Success = result,
                Message = result ? "Incident deleted successfully" : "Failed to delete incident"
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager, Maintenance")]
        [HttpGet]
        public async Task<IActionResult> GetIncidentsByFilter([FromQuery] IncidentFilterDto incidentFilterDto, CancellationToken cancellationToken = default)
        {
            var result = await incidentService.GetIncidentsByFilter(incidentFilterDto, cancellationToken);
            var response = new ResponseDto<PaginatedDataDto<IncidentListDto>>
            {
                Success = true,
                Message = "Incidents retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Maintenance")]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteIncident(int id, [FromBody] IncidentCompleteDto incidentCompleteDto, CancellationToken cancellationToken = default)
        {
            if (id != incidentCompleteDto.IncidentId)
                return BadRequest("Incident ID mismatch");
            var result = await incidentService.CompleteIncident(incidentCompleteDto, cancellationToken);
            memoryCache.Remove($"Incident_{result.IncidentId}");
            var response = new ResponseDto<IncidentDto>
            {
                Success = true,
                Message = "Incident completed successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}