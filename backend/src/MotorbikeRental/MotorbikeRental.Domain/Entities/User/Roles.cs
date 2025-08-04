using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.User
{
    public class Roles : IdentityRole<int>
    {
        [MaxLength(250)]
        public string? Description { get; set; }

        public virtual ICollection<UserCredentials> UserCredentials { get; set; }
    }
}