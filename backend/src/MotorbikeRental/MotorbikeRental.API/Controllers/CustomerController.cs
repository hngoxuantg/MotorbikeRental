using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMemoryCache memoryCache;
        public CustomerController(ICustomerService customerService, IMemoryCache memoryCache)
        {
            this.customerService = customerService;
            this.memoryCache = memoryCache;
        }
        [Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await customerService.CreateCustomer(customerCreateDto, cancellationToken);
            var response = new ResponseDto<CustomerDto>
            {
                Success = true,
                Message = "Customer created successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetCustomerById), new { id = result.CustomerId }, response);
        }
        [Authorize(Roles = "Manager,Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken = default)
        {
            var result = new CustomerDto();
            if(memoryCache.TryGetValue($"Customer_{id}", out CustomerDto? customer))
            {
                result = customer;
            }
            else
            {
                result = await customerService.GetCustomerById(id, cancellationToken);
                if(result != null)
                    memoryCache.Set($"Customer_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<CustomerDto>
            {
                Success = true,
                Message = "Customer retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default)
        {
            if(id != customerUpdateDto.CustomerId)
            {
                return BadRequest(new ResponseDto
                {
                    Success = false,
                    Message = "Customer ID in the request body does not match the ID in the URL."
                });
            }
            var result = await customerService.UpdateCustomer(customerUpdateDto, cancellationToken);
            memoryCache.Remove($"Customer_{id}");
            var response = new ResponseDto<CustomerDto>
            {
                Success = true,
                Message = "Customer updated successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerByFilter([FromQuery] CustomerFilterDto? filterDto, CancellationToken cancellationToken = default)
        {
            var result = await customerService.GetCustomerByFilter(filterDto, cancellationToken);
            var response = new ResponseDto<PaginatedDataDto<CustomerListDto>>
            {
                Success = true,
                Message = "Customers retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken = default)
        {
            var result = await customerService.DeleteCustomer(id, cancellationToken);
            memoryCache.Remove($"Customer_{id}");
            var response = new ResponseDto
            {
                Success = true,
                Message = "Customer deleted successfully"
            };
            return Ok(response);
        }
    }
}
