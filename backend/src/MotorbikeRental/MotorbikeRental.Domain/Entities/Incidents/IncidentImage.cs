using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.Incidents
{
    public class IncidentImage
    {
        public int ImageId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IncidentId { get; set; } //Mã sự cố
        public virtual Incident Incident { get; set; }

        [Required]
        [MaxLength(500)]
        [Url]
        public string ImageUrl { get; set; }
    }
}