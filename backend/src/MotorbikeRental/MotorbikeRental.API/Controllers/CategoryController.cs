using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorbikeRental.Application.DTOs.Responses;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using System.Threading.Tasks;

namespace MotorbikeRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICategoryService categoryService;
        public CategoryController(IMemoryCache memoryCache, ICategoryService categoryService)
        {
            this.memoryCache = memoryCache;
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            IEnumerable<CategoryDto> result;
            if (memoryCache.TryGetValue("Categories", out IEnumerable<CategoryDto> categories))
            {
                result = categories;
            }
            else
            {
                result = await categoryService.GetAllCategories(cancellationToken);
                if (result != null)
                    memoryCache.Set("Categories", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<IEnumerable<CategoryDto>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            var result = await categoryService.CreateCategory(categoryCreateDto, cancellationToken);
            memoryCache.Remove("Categories");
            var responseDto = new ResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category created successfully",
                Data = result
            };
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.CategoryId }, responseDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken = default)
        {
            var result = new CategoryDto();
            if (memoryCache.TryGetValue($"Category_{id}", out CategoryDto categoryDto))
            {
                result = categoryDto;
            }
            else
            {
                result = await categoryService.GetCategoryById(id, cancellationToken);
                if (result != null)
                    memoryCache.Set($"Category_{id}", result, TimeSpan.FromMinutes(10));
            }
            var response = new ResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = result
            };
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            if (id != categoryUpdateDto.CategoryId)
                return BadRequest("Category ID mismatch");

            var result = await categoryService.UpdateCategory(categoryUpdateDto, cancellationToken);
            memoryCache.Remove("Categories");
            memoryCache.Remove($"Category_{id}");
            var responseDto = new ResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = result
            };
            return Ok(responseDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken = default)
        {
            var result = await categoryService.DeleteCategory(id, cancellationToken);
            memoryCache.Remove("Categories");
            memoryCache.Remove($"Category_{id}");
            var responseDto = new ResponseDto
            {
                Success = true,
                Message = "Category deleted successfully"
            };
            return Ok(responseDto);
        }
    }
}
