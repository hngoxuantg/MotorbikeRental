using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.Interface.IServices.IDiscountServices;
using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Pagination;
using System.Security.Cryptography;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DiscountController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IDiscountService discountService;
        public DiscountController(IMemoryCache memoryCache, IDiscountService discountService)
        {
            this.memoryCache = memoryCache;
            this.discountService = discountService;
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] DiscountCreateDto discountCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await discountService.CreateDiscount(discountCreateDto);

            var responseDto = new ResponseDto<DiscountDto>
            {
                Success = true,
                Message = "Discount created successfully",
                Data = result
            };

            return CreatedAtAction(nameof(GetDiscountById), new { id = result.DiscountId }, responseDto);
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id, CancellationToken cancellationToken = default)
        {
            var result = new DiscountDto();

            if (memoryCache.TryGetValue($"Discount_{id}", out DiscountDto? discountDto))
            {
                result = discountDto;
            }
            else
            {
                result = await discountService.GetDiscountById(id, cancellationToken);

                if (result != null)
                    memoryCache.Set($"Discount_{id}", result, TimeSpan.FromMinutes(10));
            }

            var response = new ResponseDto<DiscountDto>
            {
                Success = true,
                Message = "Discount retrieved successfully",
                Data = result
            };

            return Ok(response);
        }
        [Authorize(Roles = "Manager,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetDiscountsByFilter([FromQuery] DiscountFilterDto filter, CancellationToken cancellationToken = default)
        {
            var result = await discountService.GetDiscountsByFilter(filter, cancellationToken);

            var response = new ResponseDto<PaginatedDataDto<DiscountDto>>
            {
                Success = true,
                Message = "Discounts retrieved successfully",
                Data = result
            };

            return Ok(response);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default)
        {
            if (id != discountUpdateDto.DiscountId)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Discount ID in the request body does not match the ID in the URL."
                });
            }

            var result = await discountService.UpdateDiscount(discountUpdateDto, cancellationToken);

            memoryCache.Remove($"Discount_{id}");

            var response = new ResponseDto<DiscountDto>
            {
                Success = true,
                Message = "Discount updated successfully",
                Data = result
            };

            return Ok(response);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id, CancellationToken cancellationToken = default)
        {
            await discountService.DeleteDiscount(id, cancellationToken);

            memoryCache.Remove($"Discount_{id}");

            var response = new ResponseDto
            {
                Success = true,
                Message = "Discount deleted successfully."
            };

            return Ok(response);
        }
    }
}
