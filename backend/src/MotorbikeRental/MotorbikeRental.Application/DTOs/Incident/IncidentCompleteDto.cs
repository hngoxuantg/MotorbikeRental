using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentCompleteDto
    {
        [Required(ErrorMessage = "IncidentId is required")]
        public int IncidentId { get; set; }
        [Required(ErrorMessage = "Resolved date is required")]
        public DateTime ResolvedDate { get; set; }
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; } // Ghi chú bổ sung
    }
}