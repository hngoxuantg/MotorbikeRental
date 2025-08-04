using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;

namespace MotorbikeRental.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MotorbikeController : ControllerBase
    {
        private readonly IMotorbikeService motorbikeService;
        private readonly IMemoryCache memoryCache;

        public MotorbikeController(IMotorbikeService motorbikeService, IMemoryCache memoryCache)
        {
            this.motorbikeService = motorbikeService;
            this.memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> GetMotorbikeByFilter([FromQuery] MotorbikeFilterDto? filterDto, CancellationToken cancellationToken = default)
        {
            var result = await motorbikeService.GetMotorbikesByFilter(filterDto, cancellationToken);
            var responseDto = new ResponseDto<PaginatedDataDto<MotorbikeListDto>>
            {
                Success = true,
                Message = "Motorbikes retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpGet("filter-options")]
        public async Task<IActionResult> GetMotorbikeFilterOptions(CancellationToken cancellationToken)
        {
            var result = await motorbikeService.GetMotorbikeFilterOptions(cancellationToken);
            var responseDto = new ResponseDto<MotorbikeIndexDto>
            {
                Success = true,
                Message = "Motorbikes options retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorbikeById(int id, CancellationToken cancellationToken)
        {
            var result = new MotorbikeDto();
            if (memoryCache.TryGetValue($"Motorbike_{id}", out MotorbikeDto? cacheMotorbike))
            {
                result = cacheMotorbike;
            }
            else
            {
                result = await motorbikeService.GetMotorbikeById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Motorbike_{id}", result, TimeSpan.FromMinutes(10));
            }
            var responseDto = new ResponseDto<MotorbikeDto>
            {
                Success = true,
                Message = "Motorbike retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateMotorbike([FromForm] MotorbikeCreateDto motorbikeCreateDto, CancellationToken cancellationToken)
        {
            var result = await motorbikeService.CreateMotorbike(motorbikeCreateDto, cancellationToken);
            var response = new ResponseDto<MotorbikeDto>
            {
                Success = true,
                Message = "Motorbike create successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetMotorbikeById), new { id = result.MotorbikeId }, response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditMotorbike(int id, [FromForm] MotorbikeUpdateDto motorbikeUpdateDto, CancellationToken cancellationToken)
        {
            if(id != motorbikeUpdateDto.MotorbikeId)
            {
                var errorResponse = new ResponseDto
                {
                    Success = false,
                    Message = "Id in URL and body must match"
                };
                return BadRequest(errorResponse);
            }
            var result = await motorbikeService.UpdateMotorbike(motorbikeUpdateDto, cancellationToken);
            memoryCache.Remove($"Motorbike_{id}");
            var response = new ResponseDto<MotorbikeDto>
            {
                Success = true,
                Message = "Motorbike update successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteMotorbike(int id, CancellationToken cancellationToken)
        {
            await motorbikeService.DeleteMotorbike(id, cancellationToken);
            memoryCache.Remove($"Motorbike_{id}");
            return Ok(new ResponseDto
            {
                Success = true,
                Message = "Motorbike delete successfully"
            });
        }
    }
}
