using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.IncidentEnum;

namespace MotorbikeRental.Domain.Entities.Incidents
{
    public class Incident //Sự cố
    {
        public int IncidentId { get; set; } //Mã sự cố  

        [Range(1, int.MaxValue)]
        public int ContractId { get; set; } //Mã hợp đồng
        public virtual RentalContract RentalContract { get; set; }

        [Range(1, int.MaxValue)]
        public int? MotorbikeId { get; set; } //Mã xe 
        public virtual Motorbike Motorbike { get; set; }

        [Required]
        public DateTime IncidentDate { get; set; } //Ngày bị

        [Required]
        public TypeStatus Type { get; set; } //Loại sự cố

        [Required]
        public SeverityStatus Severity { get; set; } //Mức độ nghiêm trọng

        [MaxLength(500)]
        public string? Description { get; set; } //Mô tả

        [Range(0, 999999999)]
        public decimal? DamageCost { get; set; } //Chi phí sửa chữa

        [Required]
        public bool IsResolved { get; set; } = false; //Đã giải quyết chưa?

        public DateTime? ResolvedDate { get; set; }  //Ngày giải quyết

        [Range(1, int.MaxValue)]
        public int? ReportedByEmployeeId { get; set; } //Nhân viên báo cáo
        public virtual Employee ReportedByEmployee { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; } //Ghi chú bổ sung

        [Required]
        public DateTime CreatedAt { get; set; } //Ngày tạo bản ghi

        // Mối quan hệ với IncidentImage
        public virtual ICollection<IncidentImage> Images { get; set; }
    }
}