using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Domain.Enums.IncidentEnum;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentCreateDto
    {
        [Required(ErrorMessage = "ContractId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ContractId must be a positive integer.")]
        public int ContractId { get; set; } // Mã hợp đồng
        [Required(ErrorMessage = "IncidentDate is required.")]
        public DateTime IncidentDate { get; set; } // Ngày xảy ra sự cố
        [Required(ErrorMessage = "Type is required.")]
        public TypeStatus Type { get; set; } // Loại sự cố
        [Required(ErrorMessage = "Severity is required.")]
        public SeverityStatus Severity { get; set; } // Mức độ nghiêm trọng
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; } // Mô tả sự cố
        [Range(0, 999999999, ErrorMessage = "DamageCost must be a non-negative number.")]
        public decimal? DamageCost { get; set; } // Chi phí sửa chữa (nếu có)
        public DateTime? ResolvedDate { get; set; } // Ngày giải quyết (nếu đã giải quyết)
        [Range(1, int.MaxValue, ErrorMessage = "ReportedByEmployeeId must be a positive integer.")]
        public int? ReportedByEmployeeId { get; set; } // Nhân viên báo cáo sự cố
        public List<IFormFile>? Images { get; set; } // Danh sách hình ảnh liên quan đến sự cố
    }
}
