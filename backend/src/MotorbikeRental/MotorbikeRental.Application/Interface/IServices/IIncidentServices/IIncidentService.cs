using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.DTOs.Pagination;

namespace MotorbikeRental.Application.Interface.IServices.IIncidentServices
{
    public interface IIncidentService
    {
        Task<IncidentDto> CreateIncident(IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default);
        Task<IncidentDto> UpdateBeforeComplete(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default);
        Task<IncidentDto> GetIncidentById(int id, CancellationToken cancellation = default);
        Task<IncidentDto> GetIncidentByContractId(int contractId, CancellationToken cancellation = default);
        Task<bool> DeleteIncident(int incidentId, CancellationToken cancellationToken = default);
        Task<PaginatedDataDto<IncidentListDto>> GetIncidentsByFilter(IncidentFilterDto incidentFilterDto, CancellationToken cancellationToken = default);
        Task<IncidentDto> CompleteIncident(IncidentCompleteDto incidentCompleteDto, CancellationToken cancellationToken = default);
    }
}
