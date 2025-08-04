using MotorbikeRental.Application.DTOs.ContractDto;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Application.Interface.IServices.IContractServices
{
    public interface IContractService
    {
        Task<RentalPriceResponseDto> CalculateRentalPrice(RentalPriceRequestDto priceRequestDto, CancellationToken cancellationToken = default);
        Task<ContractDto> CreateContract(ContractCreateDto contractCreate, CancellationToken cancellation = default);
        Task<bool> UpdateContractStatusActive(int contractId, CancellationToken cancellationToken = default);
        Task<bool> CancelContractByCustomer(int contractId, CancellationToken cancellationToken = default);
        Task<ContractDto> UpdateContractBeforeActivation(ContractUpdateBeforeActivationDto contractUpdate, CancellationToken cancellationToken = default);
        Task<ContractDto> GetContractById(int contractId, CancellationToken cancellationToken = default);
        Task<bool> DeleteContract(int contractId, CancellationToken cancellationToken = default);
        Task<ContractDto> ContractSettlement(ContractSettlementDto contractSettlementDto, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<ContractListDto>> GetContractFilter(ContractFilterDto contractFilterDto, CancellationToken cancellation = default);
    }
}
