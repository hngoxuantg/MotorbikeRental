using MotorbikeRental.Domain.Enums.ContractEnum;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.Payments
{
    public class PaymentProcessDto
    {
        [Required(ErrorMessage = "ContractId is required.")]
        public int ContractId { get; set; } // Mã hợp đồng
        [Required(ErrorMessage = "PaymentDate is required.")]
        public DateTime PaymentDate { get; set; } // Ngày thanh toán
        public PaymentStatus PaymentStatus { get; set; } // Trạng thái thanh toán
        [Required(ErrorMessage = "PaymentDate is required.")]
        public int EmployeeId { get; set; } // Mã nhân viên thực hiện thanh toán
    }
}
