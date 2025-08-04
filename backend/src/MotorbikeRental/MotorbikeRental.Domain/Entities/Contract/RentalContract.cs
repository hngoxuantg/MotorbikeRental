using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Domain.Entities.Contract
{
    public class RentalContract
    {
        [Required]
        public int ContractId { get; set; } // Mã hợp đồng

        [Range(1, int.MaxValue)]
        public int? CustomerId { get; set; } //Mã khách hàng
        public virtual Customer Customer { get; set; }

        [Range(1, int.MaxValue)]
        public int? MotorbikeId { get; set; } //Mã xe   
        public virtual Motorbike Motorbike { get; set; }

        [Required]
        [Range(0, 999999999)]
        public decimal DepositAmount { get; set; } // Tiền cọc

        [Required]
        [Range(1, int.MaxValue)]
        public int EmployeeId { get; set; } // Nhân viên làm hợp đồng
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime RentalDate { get; set; } //Ngày thuê

        [Required]
        public DateTime ExpectedReturnDate { get; set; } //Ngày, giờ dự kiến trả

        [Required]
        [Range(0.01, 999999999)]
        public decimal TotalAmount { get; set; } //Tổng tiền thuê

        public DateTime? ActualReturnDate { get; set; } //Ngày thực tế trả

        [Range(0, 999999999)]
        public decimal? LateReturnFee { get; set; } //Phí trả muộn

        [Range(0.1, 10.0)]
        public decimal? LateReturnFeeMultiplier { get; set; } = 2.0m;

        [Range(1, int.MaxValue)]
        public int? DiscountId { get; set; }  //Mã giảm giá
        public virtual Discount Discount { get; set; }

        [Range(0, 999999999)]
        public decimal? DiscountAmount { get; set; } //Số tiền giảm
        [Required]
        public RentalContractStatus RentalContractStatus { get; set; } //Trạng thái (Đang thuê, đã trả)     

        [Required]
        public RentalTypeStatus RentalTypeStatus { get; set; } //Thuê theo giờ - ngày

        [Required]
        public bool IdCardHeld { get; set; } // Đã giữ cccd?
        [Required]
        public bool IsPaid { get; set; } = false; //Đã thanh toán?

        [MaxLength(100)]
        public string? Note { get; set; } //Ghi chú

        public virtual Incident Incident { get; set; }
        public virtual Payment Payments { get; set; } //Danh sách thanh toán
    }
}