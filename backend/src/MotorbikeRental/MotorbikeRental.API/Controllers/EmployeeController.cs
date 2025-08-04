using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.User;
using MotorbikeRental.Application.Interface.IServices.IUserServices;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IEmployeeService employeeService;
        private readonly IRoleService roleService;
        public EmployeeController(IMemoryCache memoryCache, IEmployeeService employeeService, IRoleService roleService)
        {
            this.memoryCache = memoryCache;
            this.employeeService = employeeService;
            this.roleService = roleService;
        }
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeByFilter([FromQuery] EmployeeFilterDto? employeeFilterDto, CancellationToken cancellationToken = default)
        {
            var result = await employeeService.GetEmployeeByFilter(employeeFilterDto, cancellationToken);
            var responseDto = new ResponseDto<PaginatedDataDto<EmployeeListDto>>
            {
                Success = true,
                Message = "Employees retrieved successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id, CancellationToken cancellationToken = default)
        {
            var result = new EmployeeDto();
            if (memoryCache.TryGetValue($"Employee_{id}", out EmployeeDto? employeeDto))
            {
                result = employeeDto;
            }
            else
            {
                result = await employeeService.GetEmployeeById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Employee_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<EmployeeDto>
            {
                Success = true,
                Message = "Employee retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await employeeService.CreateEmployee(employeeCreateDto, cancellationToken);
            var response = new ResponseDto<EmployeeDto>
            {
                Success = true,
                Message = "Employee create successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetEmployeeById), new { id = result.EmployeeId }, response);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee(int id, [FromForm] EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            if (employeeUpdateDto.EmployeeId != id)
            {
                var errorResponse = new ResponseDto
                {
                    Success = false,
                    Message = "Id in URL and body must match"
                };
                return BadRequest(errorResponse);
            }
            var result = await employeeService.UpdateEmployee(employeeUpdateDto, cancellationToken);
            memoryCache.Remove($"Employee_{id}");
            var response = new ResponseDto<EmployeeDto>
            {
                Success = true,
                Message = "Employee update successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken cancellationToken = default)
        {
            await employeeService.DeleteEmployee(id, cancellationToken);
            memoryCache.Remove($"Employee_{id}");
            var response = new ResponseDto
            {
                Success = true,
                Message = "Employee delete successfully"
            };
            return Ok(response);
        }
        [HttpGet("roles")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllRole(CancellationToken cancellationToken)
        {
            var result = await roleService.GetAllRoles(cancellationToken);
            var response = new ResponseDto<IEnumerable<RoleDto>>
            {
                Success = true,
                Message = "Roles retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpDelete("{id}/avatar")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteAvatar(int id, CancellationToken cancellation = default)
        {
            var result = await employeeService.DeleteAvatar(id, cancellation);
            memoryCache.Remove($"Employee_{id}");
            var response = new ResponseDto
            {
                Success = true,
                Message = "Avatar deleted successfully"
            };
            return Ok(response);
        }
    }
}
