using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Entities.Pricing
{
    public class PriceList
    {
        public int PriceListId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MotorbikeId { get; set; } //FK table Motorbike
        public virtual Motorbike Motorbike { get; set; }

        [Required]
        [Range(0.01, 9999999)]
        public decimal HourlyRate { get; set; }//Thuê theo giờ

        [Required]
        [Range(0.01, 9999999)]
        public decimal DailyRate { get; set; }//Giá theo ngày
    }
}