using MotorbikeRental.Domain.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Domain.Entities.Pricing
{
    public class Discount_Category
    {
        public int DiscountId { get; set; } // Mã giảm giá
        public virtual Discount Discount { get; set; } // Thông tin giảm giá
        public int CategoryId { get; set; } // Mã danh mục
        public virtual Category Category { get; set; } // Thông tin danh mục
    }
}
