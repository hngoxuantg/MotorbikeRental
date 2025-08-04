using MotorbikeRental.Domain.Enums.CustomerEnum;

namespace MotorbikeRental.Application.DTOs.Customers
{
    public class CustomerListDto
    {
        public int CustomerId { get; set; } //Id khách hàng
        public string FullName { get; set; } //Tên khách hàng
        public Gender Gender { get; set; } //Giới tính
        public int RentalCount { get; set; } // Số lần thuê
        public string PhoneNumber { get; set; } //Số điện thoại
        public DateTime CreateAt { get; set; } //Ngày tạo 
    }
}