using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Discount
{
    public class DiscountFilterDto
    {
        private int pageNumber = 1;
        private int pageSize = 12;
        [MaxLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? Search { get; set; }
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value >= 1) pageNumber = value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                if(value >= 1 && value <= 100) pageSize = value;
            }
        }
        public int? CategoryId { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public bool? IsActive { get; set; } 
    }
}
