using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class RentalPriceResponseDto
    {
        public int MotorbikeId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public RentalTypeStatus RentalTypeStatus { get; set; }

        public decimal DepositAmount { get; set; } // Số tiền đặt cọc
        public string? DiscountName { get; set; } // Tên chương trình giảm giá (nếu có)
        public int? DiscountValue { get; set; } // Giá trị giảm giá (nếu có), theo %
        public decimal RentalPrice { get; set; } // Giá thuê xe 
        public decimal? DiscountAmount { get; set; } // Số tiền giảm giá (nếu có)
        public decimal TotalPrice { get; set; } // Tổng
    }
}
