using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal DepositAmount { get; set; } 
    }
}