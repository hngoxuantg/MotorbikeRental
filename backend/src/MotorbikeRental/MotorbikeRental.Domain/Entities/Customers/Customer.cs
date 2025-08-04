using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Enums.CustomerEnum;

namespace MotorbikeRental.Domain.Entities.Customers
{
    public class Customer
    {
        public int CustomerId { get; set; } //Id khách hàng

        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } //Tên khách hàng
        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string IdNumber { get; set; } //Số CCCD

        [Required]
        [MaxLength(13)]
        public string PhoneNumber { get; set; } //Số điện thoại

        [EmailAddress]
        [MaxLength(50)]
        public string? Email { get; set; } //Email

        [Required]
        public DateTime CreateAt { get; set; } //Ngày tạo 

        public virtual ICollection<RentalContract> RentalContracts { get; set; }
    }
}