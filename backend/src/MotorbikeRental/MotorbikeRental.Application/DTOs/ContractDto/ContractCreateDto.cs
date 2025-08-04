using MotorbikeRental.Domain.Enums.ContractEnum;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractCreateDto
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "MotorbikeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MotorbikeId must be a positive integer.")]
        public int MotorbikeId { get; set; }

        [Required(ErrorMessage = "EmployeeId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be a positive integer.")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "RentalDate is required.")]
        public DateTime RentalDate { get; set; }
        [Required(ErrorMessage = "ReturnDate is required.")]
        public DateTime ExpectedReturnDate { get; set; }
        [Required(ErrorMessage = "TotalAmount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a non-negative number.")]
        public decimal TotalAmount { get; set; } // Tổng tiền thuê
        [Range(1, int.MaxValue, ErrorMessage = "DiscountId must be a positive integer.")]
        public int? DiscountId { get; set; } // Mã giảm giá (nếu có)
        [Required(ErrorMessage = "RentalTypeStatus is required.")]
        public RentalTypeStatus RentalTypeStatus { get; set; } // Loại hình thuê (theo giờ, theo ngày, v.v.)
        [Required(ErrorMessage = "IsCardHeld is required.")]
        public bool IdCardHeld { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public RentalContractStatus Status { get; set; } // Trạng thái hợp đồng
        public string? Note { get; set; } // Ghi chú (nếu có)
    }
}
