using MotorbikeRental.Domain.Enums.ContractEnum;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class ContractFilterDto
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
        public RentalContractStatus? Status { get; set; }
        [MaxLength(50, ErrorMessage = "Search term cannot exceed 50 characters.")]
        public string? Search { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
