using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class MotorbikeIndexDto
    {
        public IEnumerable<CategoryDto> CategoriesDto { get; set; }
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<MotorbikeStatus> motorbikeStatuses { get; set; } = Enum.GetValues<MotorbikeStatus>();
    }
}