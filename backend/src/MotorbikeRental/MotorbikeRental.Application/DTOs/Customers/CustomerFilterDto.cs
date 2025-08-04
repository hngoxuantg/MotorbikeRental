using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Customers
{
    public class CustomerFilterDto
    {
        private int pageNumber = 1;
        private int pageSize = 12;
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if(value >= 1) pageNumber = value;
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
        [MaxLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? Search { get; set; }
    }
}
