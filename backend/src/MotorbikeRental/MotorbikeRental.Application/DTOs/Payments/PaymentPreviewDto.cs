namespace MotorbikeRental.Application.DTOs.Payments
{
    public class PaymentPreviewDto
    {
        public int ContractId { get; set; }
        public string CustomerName { get; set; }
        public decimal? ContractIndemnity { get; set; } // Bồi thường hợp đồng
        public decimal? IncidentFineAmount { get; set; } // Số tiền phạt sự cố
        public decimal Amount { get; set; } // Tổng tiền thanh toán
    }
}
