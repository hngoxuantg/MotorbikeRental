using MotorbikeRental.Domain.Enums.IncidentEnum;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentDto
    {
        public int IncidentId { get; set; } // Mã sự cố
        public int? ContractId { get; set; } // Mã hợp đồng
        public string CustomerName { get; set; } // Tên khách hàng
        public int? MotorbikeId { get; set; } // Mã xe
        public string? MotorbikeName { get; set; } // Tên xe
        public string? MotorbikeImage { get; set; } // Hình ảnh xe
        public DateTime IncidentDate { get; set; } // Ngày xảy ra sự cố
        public TypeStatus Type { get; set; } // Loại sự cố
        public SeverityStatus Severity { get; set; } // Mức độ nghiêm trọng
        public string? Description { get; set; } // Mô tả sự cố
        public decimal? DamageCost { get; set; } // Chi phí sửa chữa
        public bool IsResolved { get; set; } // Đã giải quyết chưa?
        public DateTime? ResolvedDate { get; set; } // Ngày giải quyết
        public string? ReportedByEmployeeName { get; set; } // Tên nhân viên báo cáo
        public string? Notes { get; set; } // Ghi chú bổ sung
        public DateTime CreatedAt { get; set; } // Ngày tạo bản ghi
        public List<string>? Images { get; set; } // Danh sách hình ảnh sự cố
    }
}
