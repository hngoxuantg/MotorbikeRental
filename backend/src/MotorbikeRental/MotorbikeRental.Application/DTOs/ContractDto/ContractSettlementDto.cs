using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractSettlementDto
    {
        [Required(ErrorMessage = "ContractId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ContractId must be a positive integer.")]
        public int ContractId { get; set; } // Mã hợp đồng
        [Required(ErrorMessage = "ActualReturnDate is required.")]
        public DateTime ActualReturnDate { get; set; } // Ngày thực tế trả xe
    }
}
