using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Domain.Entities.Contract
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int? ContractId { get; set; } //FK Hợp đồng
        public virtual RentalContract RentalContract { get; set; }

        [Required]
        [Range(0.01, 999999999)]
        public decimal Amount { get; set; } //Tổng tiền

        [Required]
        public DateTime PaymentDate { get; set; } //Ngày thanh toán

        [Required]
        public PaymentStatus PaymentStatus { get; set; } //Tiền mặt, chuyển khoản?

        [Range(0, 999999999)]
        public decimal? ContractIndemnity { get; set; } //Tiền phạt vi phạm hợp đồng
        [Range(0, double.MaxValue)]
        public decimal? IncidentFineAmount { get; set; } //Tền phạt vi phạm hợp đồng do sự cố
        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}