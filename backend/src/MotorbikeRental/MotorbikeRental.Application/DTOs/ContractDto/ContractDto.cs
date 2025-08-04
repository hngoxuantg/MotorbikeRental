using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractDto
    {
        public string ContractId { get; set; } // Mã hợp đồng
        public int CustomerId { get; set; } // Mã khách hàng
        public string CustomerFullName { get; set; } // Tên khách hàng
        public int MotorbikeId { get; set; } // Mã xe
        public string MotorbikeName { get; set; } // Tên xe
        public string? MotorbikeImageUrl { get; set; } // Hình ảnh xe
        public string MotorbikeLicensePlate { get; set; } // Biển số xe
        public string CategoryName { get; set; } // Tên loại xe
        public decimal DepositAmount { get; set; } // Tiền cọc
        public int EmployeeId { get; set; } // Mã nhân viên
        public string EmployeeFullName { get; set; } // Tên nhân viên
        public DateTime RentalDate { get; set; } // Ngày thuê
        public DateTime ExpectedReturnDate { get; set; } // Ngày, giờ dự kiến trả
        public decimal TotalAmount { get; set; } // Tổng tiền thuê
        public DateTime? ActualReturnDate { get; set; } // Ngày thực tế trả
        public decimal? LateReturnFee { get; set; } // Phí trả muộn
        public decimal LateReturnFeeMultiplier { get; set; } = 2.0m; // Hệ số phí trả muộn
        public int DiscountId { get; set; } = 0; // Mã giảm giá
        public string DiscountName { get; set; } //Tên chương trình giảm giá
        public decimal? DiscountAmount { get; set; } // Số tiền giảm 
        public RentalContractStatus RentalContractStatus { get; set; } // Trạng thái hợp đồng (Đang thuê, đã trả)
        public RentalTypeStatus RentalTypeStatus { get; set; } // Thuê theo giờ - ngày
        public bool IdCardHeld { get; set; } //Đã giữ CCCD?
        public bool IsPaid { get; set; }
        public string? Note { get; set; } //Ghi chú
    }
}
