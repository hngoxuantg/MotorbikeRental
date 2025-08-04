using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Enums.CustomerEnum;

namespace MotorbikeRental.Application.DTOs.Customers;

public class CustomerDto
{
    public int CustomerId { get; set; } //Id khách hàng
    public string FullName { get; set; } //Tên khách hàng
    public Gender Gender { get; set; }
    public int RentalCount { get; set; } // Số lần thuê
    public string IdNumber { get; set; } //Số CCCD
    public string PhoneNumber { get; set; } //Số điện thoại
    public string? Email { get; set; } //Email
    public DateTime CreateAt { get; set; } //Ngày tạo 
}