using MotorbikeRental.Domain.Enums.UserEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeListDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Avatar {  get; set; }
        public string Salary { get; set; }
        public string RoleName { get; set; }
        public EmployeeStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
