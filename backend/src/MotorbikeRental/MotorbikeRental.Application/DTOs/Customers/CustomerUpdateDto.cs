using MotorbikeRental.Domain.Enums.CustomerEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Customers
{
    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int CustomerId { get; set; } //Id khách hàng     
        [Required(ErrorMessage = "Full name is required"), MaxLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; } //Tên khách hàng
        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; } //Giới tính
        [Required(ErrorMessage = "Phone number is required"), MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string IdNumber { get; set; } //Số CCCD
        [Required(ErrorMessage = "Phone number is required"), MaxLength(13, ErrorMessage = "Phone number cannot exceed 13 characters")]
        public string PhoneNumber { get; set; } //Số điện thoại
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format"), MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string Email { get; set; } //Email
    }
}
