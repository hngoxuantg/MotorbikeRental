using Microsoft.AspNetCore.Identity;

namespace MotorbikeRental.Domain.Entities.User
{
    public class UserCredentials : IdentityUser<int>
    {
        public int? RoleId { get; set; } //FK Table Roles
        public virtual Roles Roles { get; set; }
        public DateTime? LastLogin { get; set; }     //Thời gian đăng nhập gần nhất
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}