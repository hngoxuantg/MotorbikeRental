using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;

namespace MotorbikeRental.Application.Interface.IValidators.IIncidentValidators
{
    public interface IIncidentValidator
    {
        Task<bool> ValidateForCreate(IncidentCreateDto incidentCreateDto, RentalContract rentalContract, CancellationToken cancellationToken = default);
        Task<bool> ValidateForUpdateBeforeActivation(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, RentalContract rentalContract, CancellationToken cancellationToken = default);
        bool ValidateForCompleteIncident(Incident incident, RentalContract rentalContract, IncidentCompleteDto incidentCompleteDto);
    }
}