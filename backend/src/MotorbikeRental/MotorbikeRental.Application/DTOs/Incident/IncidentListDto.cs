using MotorbikeRental.Domain.Enums.IncidentEnum;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentListDto
    {
        public int IncidentId { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime CreateAt { get; set; }
        public TypeStatus Type { get; set; }
        public decimal? DamageCost { get; set; }
        public IEnumerable<string>? Images { get; set; }
    }
}