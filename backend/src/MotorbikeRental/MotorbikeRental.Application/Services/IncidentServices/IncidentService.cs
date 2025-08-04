using MotorbikeRental.Application.DTOs.Incident;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using AutoMapper;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Application.Interface.IValidators.IIncidentValidators;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.DTOs.Pagination;
using System.Threading.Tasks;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Enums.ContractEnum;
using System.Linq.Expressions;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.IncidentServices
{
    public class IncidentService : IIncidentService
    {
        private readonly IMapper mapper;
        private readonly IIncidentValidator incidentValidator;
        private readonly IFileService fileService;
        private readonly IUnitOfWork unitOfWork;
        public IncidentService(IUnitOfWork unitOfWork, IMapper mapper, IIncidentValidator incidentValidator, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.incidentValidator = incidentValidator;
            this.fileService = fileService;
        }
        public async Task<IncidentDto> CreateIncident(IncidentCreateDto incidentCreateDto, CancellationToken cancellationToken = default)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                RentalContract rentalContract = await unitOfWork.RentalContractRepository.GetById(incidentCreateDto.ContractId, cancellationToken) ?? throw new NotFoundException("Rental contract not found");
                Employee employee = await unitOfWork.EmployeeRepository.GetEmployeeBasicInfoById(incidentCreateDto.ReportedByEmployeeId.Value, cancellationToken) ?? throw new NotFoundException("Employee not found");
                Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetById(rentalContract.MotorbikeId.Value, cancellationToken) ?? throw new NotFoundException("Motorbike not found");
                Customer? customer = await unitOfWork.CustomerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
                await incidentValidator.ValidateForCreate(incidentCreateDto, rentalContract, cancellationToken);
                Incident incident = mapper.Map<Incident>(incidentCreateDto);
                incident.MotorbikeId = motorbike.MotorbikeId;
                incident.CreatedAt = DateTime.UtcNow;
                if (incidentCreateDto.Images != null)
                {
                    List<string> images = await fileService.SaveImages(incidentCreateDto.Images, "Incident", cancellationToken);
                    incident.Images = images.Select(path => new IncidentImage
                    {
                        ImageUrl = path
                    }).ToList();
                }
                IncidentDto incidentDto = mapper.Map<IncidentDto>(await unitOfWork.IncidentRepository.Create(incident, cancellationToken));
                if (!incidentCreateDto.ResolvedDate.HasValue)
                {
                    rentalContract.RentalContractStatus = RentalContractStatus.ProcessingIncident;
                    motorbike.Status = MotorbikeStatus.Damaged;
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                MapToIncidentDto(incidentDto, customer, employee, motorbike);
                return incidentDto;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        public async Task<IncidentDto> UpdateBeforeComplete(IncidentUpdateBeforeCompleteDto incidentUpdateBeforeCompleteDto, CancellationToken cancellationToken = default)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                Incident incident = await unitOfWork.IncidentRepository.GetIncidentByIdWithIncludes(incidentUpdateBeforeCompleteDto.IncidentId, true, cancellationToken) ?? throw new NotFoundException("Incident not found");
                RentalContract rentalContract = await unitOfWork.RentalContractRepository.GetContractById(incidentUpdateBeforeCompleteDto.ContractId, cancellationToken) ?? throw new NotFoundException("Rental contract not found");
                Employee employee = await unitOfWork.EmployeeRepository.GetEmployeeBasicInfoById(incidentUpdateBeforeCompleteDto.ReportedByEmployeeId.Value, cancellationToken) ?? throw new NotFoundException("Employee not found");
                Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetByIdWithIncludes(rentalContract.MotorbikeId.Value, cancellationToken) ?? throw new NotFoundException("Motorbike not found");
                Customer? customer = await unitOfWork.CustomerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
                await incidentValidator.ValidateForUpdateBeforeActivation(incidentUpdateBeforeCompleteDto, rentalContract, cancellationToken);
                mapper.Map(incidentUpdateBeforeCompleteDto, incident);
                if (incidentUpdateBeforeCompleteDto.Images != null)
                {
                    if (incident.Images != null)
                    {
                        if (!fileService.DeleteFiles(incident.Images.Select(im => im.ImageUrl).ToList())) throw new Exception("Failed to delete file");
                        incident.Images.Clear();
                        await unitOfWork.IncidentRepository.SaveChangeAsync(cancellationToken);
                    }
                    List<string> images = await fileService.SaveImages(incidentUpdateBeforeCompleteDto.Images, "Incident", cancellationToken);
                    incident.Images = images.Select(path => new IncidentImage
                    {
                        ImageUrl = path
                    }).ToList();
                }
                unitOfWork.IncidentRepository.UpdateEntity(incident);
                incident.MotorbikeId = rentalContract.MotorbikeId;
                IncidentDto incidentDto = mapper.Map<IncidentDto>(incident);
                MapToIncidentDto(incidentDto, customer, employee, motorbike);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return incidentDto;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        public async Task<IncidentDto> CompleteIncident(IncidentCompleteDto incidentCompleteDto, CancellationToken cancellationToken = default)
        {
            Incident incident = await unitOfWork.IncidentRepository.GetById(incidentCompleteDto.IncidentId, cancellationToken) ?? throw new NotFoundException($"Incident with id {incidentCompleteDto.IncidentId} not found");
            RentalContract rentalContract = await unitOfWork.RentalContractRepository.GetContractById(incident.ContractId, cancellationToken) ?? throw new NotFoundException("Rental contract not found");
            incidentValidator.ValidateForCompleteIncident(incident, rentalContract, incidentCompleteDto);
            Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetById(rentalContract.MotorbikeId.Value, cancellationToken);
            motorbike.Status = MotorbikeStatus.Available;
            Employee employee = await unitOfWork.EmployeeRepository.GetEmployeeBasicInfoById(incident.ReportedByEmployeeId.Value, cancellationToken);
            Customer? customer = await unitOfWork.CustomerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellationToken);
            if (incidentCompleteDto.Notes != null)
                incident.Notes = incidentCompleteDto.Notes;
            incident.IsResolved = true;
            incident.ResolvedDate = incidentCompleteDto.ResolvedDate;
            await unitOfWork.IncidentRepository.SaveChangeAsync(cancellationToken);
            IncidentDto incidentDto = mapper.Map<IncidentDto>(incident);
            MapToIncidentDto(incidentDto, customer, employee, motorbike);
            return incidentDto;
        }
        public async Task<IncidentDto> GetIncidentByContractId(int contractId, CancellationToken cancellation = default)
        {
            return await GetIncident(i => i.ContractId == contractId, cancellation);
        }
        public async Task<IncidentDto> GetIncidentById(int incidentId, CancellationToken cancellation = default)
        {
            return await GetIncident(i => i.IncidentId == incidentId, cancellation);
        }
        private async Task<IncidentDto> GetIncident(Expression<Func<Incident, bool>> expression, CancellationToken cancellation = default)
        {
            Incident incident = await unitOfWork.IncidentRepository.GetWithIncludes(expression,
                cancellation,
                i => i.Images) ?? throw new NotFoundException("Incident not found");
            RentalContract? rentalContract = await unitOfWork.RentalContractRepository.GetContractById(incident.ContractId, cancellation);
            Customer? customer = await unitOfWork.CustomerRepository.GetCustomerBasicInfoById(rentalContract.CustomerId.Value, cancellation);
            Employee employee = await unitOfWork.EmployeeRepository.GetEmployeeBasicInfoById(incident.ReportedByEmployeeId.Value, cancellation) ?? throw new NotFoundException("Employee not found");
            Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetByIdWithIncludes(incident.MotorbikeId.Value, cancellation) ?? throw new NotFoundException("Motorbike not found");
            IncidentDto incidentDto = mapper.Map<IncidentDto>(incident);
            MapToIncidentDto(incidentDto, customer, employee, motorbike);
            return incidentDto;
        }

        public async Task<PaginatedDataDto<IncidentListDto>> GetIncidentsByFilter(IncidentFilterDto incidentFilterDto, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Incident> incidents, int totalCount) = await unitOfWork.IncidentRepository.GetIncidentsByFilter(
                    incidentFilterDto.PageNumber, incidentFilterDto.PageSize, incidentFilterDto.MotorbikeStatus, incidentFilterDto.FromDate, incidentFilterDto.ToDate, incidentFilterDto.IsResolved, cancellationToken);
            return new PaginatedDataDto<IncidentListDto>(mapper.Map<IEnumerable<IncidentListDto>>(incidents), totalCount);
        }
        public async Task<bool> DeleteIncident(int incidentId, CancellationToken cancellationToken = default)
        {
            Incident? incident = await unitOfWork.IncidentRepository.GetIncidentByIdWithIncludes(incidentId, true, cancellationToken) ?? throw new NotFoundException("Incident not found");
            if (incident.Images != null && !fileService.DeleteFiles(incident.Images.Select(im => im.ImageUrl).ToList())) throw new Exception("Failed to delete file");
            await unitOfWork.IncidentRepository.Delete(incident, cancellationToken);
            return true;
        }
        private void MapToIncidentDto(IncidentDto incidentDto, Customer customer, Employee employee, Motorbike motorbike)
        {
            incidentDto.CustomerName = customer.FullName;
            incidentDto.MotorbikeName = motorbike.MotorbikeName;
            incidentDto.MotorbikeImage = motorbike.ImageUrl;
            incidentDto.ReportedByEmployeeName = employee.FullName;
        }
    }
}
