using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.DTOs.Payments
{
    public class PaymentDto
    {
        public int PaymentId { get; set; } // Mã thanh toán
        public int ContractId { get; set; } // Mã hợp đồng
        public string CustomerName { get; set; } // Tên khách hàng
        public decimal Amount { get; set; } // Số tiền thanh toán
        public DateTime PaymentDate { get; set; } // Ngày thanh toán
        public PaymentStatus PaymentStatus { get; set; } // Tiền mặt, Chuyển khoản?
        public bool IsIncident { get; set; } // Có sự cố không?
        public decimal? ContractIndemnity { get; set; } //Tiền phạt vi phạm hợp đồng
        public decimal? IncidentFineAmount { get; set; } //Tền phạt vi phạm hợp đồng do sự cố
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } // Tên nhân viên thực hiện thanh toán
    }
}
