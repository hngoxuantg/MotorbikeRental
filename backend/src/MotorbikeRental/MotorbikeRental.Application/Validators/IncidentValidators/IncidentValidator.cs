using System.Threading.Tasks;
using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IValidators.IIncidentValidators;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;

namespace MotorbikeRental.Application.Validators.IncidentValidators
{
    public class IncidentValidator : IIncidentValidator
    {
        private readonly IUnitOfWork unitOfWork;
        public IncidentValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> ValidateForCreate(IncidentCreateDto incidentCreateDto, RentalContract rentalContract, CancellationToken cancellationToken = default)
        {
            if (await unitOfWork.IncidentRepository.IsExists(nameof(Incident.ContractId), incidentCreateDto.ContractId, cancellationToken))
                throw new BusinessRuleException("An incident for this contract already exists");
            if (rentalContract.RentalContractStatus != RentalContractStatus.Active)
                throw new BusinessRuleException("Cannot create incident for a contract that is not active");
            if (incidentCreateDto.IncidentDate > DateTime.UtcNow)
                throw new BusinessRuleException("Incident date cannot be in the future");
            if (incidentCreateDto.ResolvedDate.HasValue && incidentCreateDto.ResolvedDate.Value < incidentCreateDto.IncidentDate)
                throw new BusinessRuleException("Resolved date cannot be before incident date");
            if (incidentCreateDto.ResolvedDate.HasValue && incidentCreateDto.ResolvedDate.Value > DateTime.UtcNow)
                throw new BusinessRuleException("Resolved date cannot be in the future");
            return true;
        }
        public async Task<bool> ValidateForUpdateBeforeActivation(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, RentalContract rentalContract, CancellationToken cancellationToken = default)
        {
            if (await unitOfWork.IncidentRepository.IsExistsContractIdForUpdate(incidentUpdateBeforeCompleteDto.IncidentId, rentalContract.ContractId, cancellationToken))
                throw new BusinessRuleException("An incident for this contract already exists");
            if (rentalContract.RentalContractStatus != RentalContractStatus.Active)
                throw new BusinessRuleException("Cannot update incident for a contract that is not active");
            if (incidentUpdateBeforeCompleteDto.IncidentDate > DateTime.UtcNow.Date)
                throw new BusinessRuleException("Incident date cannot be in the future");
            if (incidentUpdateBeforeCompleteDto.ResolvedDate.HasValue && incidentUpdateBeforeCompleteDto.ResolvedDate.Value < incidentUpdateBeforeCompleteDto.IncidentDate)
                throw new BusinessRuleException("Resolved date cannot be before incident date");
            if (incidentUpdateBeforeCompleteDto.ResolvedDate.HasValue && incidentUpdateBeforeCompleteDto.ResolvedDate.Value > DateTime.UtcNow.Date)
                throw new BusinessRuleException("Resolved date cannot be in the future");
            return true;
        }
        public bool ValidateForCompleteIncident(Incident incident, RentalContract rentalContract, IncidentCompleteDto incidentCompleteDto)
        {
            if (incident.IsResolved)
                throw new BusinessRuleException("Incident is already resolved");
            if (incident.ResolvedDate < incidentCompleteDto.ResolvedDate)
                throw new BusinessRuleException("Resolved date cannot be before incident date");
            if (rentalContract.RentalContractStatus != RentalContractStatus.Completed)
                throw new BusinessRuleException("Cannot complete incident for a contract that is not completed");
            return true;
        }
    }
}