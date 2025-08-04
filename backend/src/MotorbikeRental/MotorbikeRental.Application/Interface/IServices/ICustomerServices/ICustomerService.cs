using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.DTOs.Pagination;

namespace MotorbikeRental.Application.Interface.IServices.ICustomerServices
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomer(CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default);
        Task<CustomerDto> GetCustomerById(int id, CancellationToken cancellationToken = default);
        Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<CustomerListDto>> GetCustomerByFilter(CustomerFilterDto filterDto, CancellationToken cancellationToken = default);
        Task<CustomerDto> UpdateCustomer(CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default);
    }
}

