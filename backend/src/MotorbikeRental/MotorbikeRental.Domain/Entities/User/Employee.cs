using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Domain.Entities.User
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(30)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Url]
        [StringLength(500)]
        public string? Avatar { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Range(0, 999999999)]
        public decimal? Salary { get; set; }
        [Required]
        public EmployeeStatus Status { get; set; }

        public virtual UserCredentials UserCredentials { get; set; }
        public virtual ICollection<MaintenanceRecord>? MaintenanceRecords { get; set; }
        public virtual ICollection<RentalContract> RentalContracts { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}