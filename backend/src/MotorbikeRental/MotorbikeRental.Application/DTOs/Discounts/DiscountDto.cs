using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Discount
{
    public class DiscountDto
    {
        public int DiscountId { get; set; } // Mã giảm giá
        public string Name { get; set; }
        public List<string> CategoryNames { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; } // Giảm theo %
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true; // Đang áp dụng hay không
    }
}
