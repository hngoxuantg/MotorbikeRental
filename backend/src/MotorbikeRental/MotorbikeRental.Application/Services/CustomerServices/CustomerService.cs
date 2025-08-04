using AutoMapper;
using MotorbikeRental.Application.DTOs.Customers;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerValidator customerValidator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CustomerService(ICustomerValidator customerValidator, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.customerValidator = customerValidator;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<CustomerDto> CreateCustomer(CustomerCreateDto customerCreateDto, CancellationToken cancellationToken = default)
        {
            await customerValidator.ValidateForCreate(customerCreateDto);
            return mapper.Map<CustomerDto>(await unitOfWork.CustomerRepository.Create(mapper.Map<Customer>(customerCreateDto), cancellationToken));
        }
        public async Task<CustomerDto> GetCustomerById(int id, CancellationToken cancellationToken = default)
        {
            Customer? customer = await unitOfWork.CustomerRepository.GetByIdWithIncludes(id, cancellationToken);
            if (customer == null)
                throw new NotFoundException($"Customer with id {id} not found");
            customer.CreateAt = DateTime.UtcNow;
            CustomerDto customerDto = mapper.Map<CustomerDto>(customer);
            return customerDto;
        }
        public async Task<PaginatedDataDto<CustomerListDto>> GetCustomerByFilter(CustomerFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Customer> customer, int totalCount) = await unitOfWork.CustomerRepository.GetFilterData(filterDto.Search, filterDto.PageNumber, filterDto.PageSize, cancellationToken);
            return new PaginatedDataDto<CustomerListDto>(mapper.Map<IEnumerable<CustomerListDto>>(customer), totalCount);
        }
        public async Task<CustomerDto> UpdateCustomer(CustomerUpdateDto customerUpdateDto, CancellationToken cancellationToken = default)
        {
            await customerValidator.ValidateForUpdate(customerUpdateDto, cancellationToken);
            Customer customer = await unitOfWork.CustomerRepository.GetByIdWithIncludes(customerUpdateDto.CustomerId, cancellationToken);
            mapper.Map(customerUpdateDto, customer);
            await unitOfWork.CustomerRepository.Update(customer, cancellationToken);
            return mapper.Map<CustomerDto>(customer);
        }
        public async Task<bool> DeleteCustomer(int id, CancellationToken cancellationToken = default)
        {
            Customer customer = await unitOfWork.CustomerRepository.GetById(id, cancellationToken);
            if (customer == null)
                throw new NotFoundException("Customer not found");
            await unitOfWork.CustomerRepository.Delete(customer, cancellationToken);
            return true;
        }
    }
}

