using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractListDto
    {
        public int ContractId { get; set; } // Mã hợp đồng
        public string CustomerName { get; set; }
        public string MotorbikeName { get; set; }
        public string? ImageUrlMotorbike { get; set; }
        public RentalTypeStatus RentalTypeStatus { get; set; } // Loại hình thuê (theo giờ, theo ngày, v.v.)
        public DateTime RentalDate { get; set; } // Ngày thuê
        public DateTime ExpectedReturnDate { get; set; } // Ngày dự kiến trả xe
        public DateTime? ActualReturnDate { get; set; } // Ngày thực tế trả xe (nếu đã trả)
        public decimal TotalAmount { get; set; } // Tổng tiền thuê
        public string? DiscountName { get; set; } // Tên giảm giá (nếu có)
        public bool IsPaid { get; set; } // Đã thanh toán?
        public RentalContractStatus Status { get; set; } // Trạng thái hợp đồng
    }
}
