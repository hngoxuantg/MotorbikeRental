using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractUpdateDto
    {
        [Required(ErrorMessage = "ContractId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ContractId must be a positive integer.")]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "CustomerId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "MotorbikeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MotorbikeId must be a positive integer.")]
        public int MotorbikeId { get; set; }

    }
}
