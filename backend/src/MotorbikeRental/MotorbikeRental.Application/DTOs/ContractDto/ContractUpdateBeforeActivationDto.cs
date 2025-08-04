using MotorbikeRental.Domain.Enums.ContractEnum;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractUpdateBeforeActivationDto
    {
        [Required(ErrorMessage = "ContractId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ContractId must be a positive integer.")]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "CustomerId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer.")]
        public int MotorbikeId { get; set; }
        [Required(ErrorMessage = "RentalDate is required.")]
        public DateTime RentalDate { get; set; }
        [Required(ErrorMessage = "ExpectedReturnDate is required.")]
        public DateTime ExpectedReturnDate { get; set; }
        [Required(ErrorMessage = "TotalAmount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a non-negative number.")]
        public decimal TotalAmount { get; set; } // Tổng tiền thuê
        [Range(1, int.MaxValue, ErrorMessage = "DiscountId must be a positive integer.")]
        public int? DiscountId { get; set; } // Mã giảm giá (nếu có)
        [Required(ErrorMessage = "RentalTypeStatus is required.")]  
        public RentalTypeStatus RentalTypeStatus { get; set; } // Loại hình thuê (theo giờ, theo ngày, v.v.)
        [MaxLength(100, ErrorMessage = "Notes cannot exceed 100 characters.")]
        public string? Note { get; set; } // Ghi chú (nếu có)
    }
}
