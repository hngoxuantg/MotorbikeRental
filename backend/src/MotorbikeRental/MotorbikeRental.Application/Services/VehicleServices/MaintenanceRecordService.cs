using AutoMapper;
using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class MaintenanceRecordService : IMaintenanceRecordService
    {
        private readonly IMaintenanceRecordValidator maintenanceRecordValidator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MaintenanceRecordService(IUnitOfWork unitOfWork, IMaintenanceRecordValidator maintenanceRecordValidator, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.maintenanceRecordValidator = maintenanceRecordValidator;
            this.mapper = mapper;
        }
        public async Task<bool> CreateMaintenanceRecord(MaintenanceRecordCreateDto maintenanceRecordCreateDto, CancellationToken cancellationToken = default)
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetWithIncludes(m => m.MotorbikeId == maintenanceRecordCreateDto.MotorbikeId,
                cancellationToken,
                m => m.MotorbikeMaintenanceInfo)
                ?? throw new KeyNotFoundException("Motorbike not found");
                await maintenanceRecordValidator.ValidateForCreate(motorbike, maintenanceRecordCreateDto, cancellationToken);
                MaintenanceRecord maintenanceRecord = mapper.Map<MaintenanceRecord>(maintenanceRecordCreateDto);
                maintenanceRecord.CreatedAt = DateTime.UtcNow.Date;
                motorbike.Status = MotorbikeStatus.UnderMaintenance;
                if (motorbike.MotorbikeMaintenanceInfo == null)
                {
                    motorbike.MotorbikeMaintenanceInfo = new MotorbikeMaintenanceInfo
                    {
                        LastMaintenanceDate = maintenanceRecordCreateDto.MaintenanceDate.Date,
                        NextMaintenanceDate = null,
                        MaintenanceCount = 1
                    };
                }
                else
                {
                    motorbike.MotorbikeMaintenanceInfo.LastMaintenanceDate = maintenanceRecordCreateDto.MaintenanceDate;
                    motorbike.MotorbikeMaintenanceInfo.NextMaintenanceDate = null;
                    motorbike.MotorbikeMaintenanceInfo.MaintenanceCount += 1;
                }
                unitOfWork.MotorbikeRepository.UpdateEntity(motorbike);
                unitOfWork.MaintenanceRecordRepository.AddEntity(maintenanceRecord);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        public async Task<MaintenanceRecordDto> MaintenanceRecordComplete(MaintenanceCompletionDto maintenanceCompletionDto, CancellationToken cancellationToken = default)
        {
            MaintenanceRecord maintenanceRecord = await unitOfWork.MaintenanceRecordRepository.GetMaintenanceByMotorbikeId(maintenanceCompletionDto.MotorbikeId, cancellationToken) ?? throw new NotFoundException("MaintenanceRecord not found");
            maintenanceRecordValidator.ValidateForComplete(maintenanceRecord, maintenanceCompletionDto);
            mapper.Map(maintenanceCompletionDto, maintenanceRecord);
            maintenanceRecord.IsCompleted = true;
            maintenanceRecord.Motorbike.Status = MotorbikeStatus.Available;
            maintenanceRecord.Motorbike.MotorbikeMaintenanceInfo.NextMaintenanceDate = maintenanceCompletionDto.NextMaintenanceDate.Date;
            await unitOfWork.MaintenanceRecordRepository.Update(maintenanceRecord, cancellationToken);
            return mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }
        public async Task<PaginatedDataDto<MaintenanceRecordDto>> GetMaintenanceRecordByFilter(MaintenanceRecordFilterDto maintenanceRecordFilterDto, CancellationToken cancellationToken = default)
        {
            (IEnumerable<MaintenanceRecord> maintenanceRecords, int totalCount) = await unitOfWork.MaintenanceRecordRepository.GetFilterData(maintenanceRecordFilterDto.PageNumber,
                maintenanceRecordFilterDto.PageSize,
                maintenanceRecordFilterDto.IsCompleted,
                maintenanceRecordFilterDto.FromDate,
                maintenanceRecordFilterDto.ToDate,
                maintenanceRecordFilterDto.Search,
                cancellationToken);
            return new PaginatedDataDto<MaintenanceRecordDto>(mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords), totalCount);
        }
        public async Task<MaintenanceRecordDto> GetById(int id, CancellationToken cancellationToken)
        {
            return mapper.Map<MaintenanceRecordDto>(await unitOfWork.MaintenanceRecordRepository.GetWithIncludes(m => m.MaintenanceRecordId == id,
                cancellationToken,
                m => m.Employee,
                m => m.Motorbike)
                ?? throw new NotFoundException("MaintenanceRecord not found"));
        }
        public async Task<MaintenanceMotorbikeDto> GetMotorbikesWithMaintenanceInfo(int motorbikeId, CancellationToken cancellationToken = default)
        {
            Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetWithIncludes(m => m.MotorbikeId == motorbikeId,
                cancellationToken,
                m => m.MotorbikeMaintenanceInfo,
                m => m.Category) ?? throw new NotFoundException("Motorbike not found");
            return mapper.Map<MaintenanceMotorbikeDto>(motorbike);
        }
        public async Task<PaginatedDataDto<MaintenanceMotorbikeDto>> GetMotorbikesWithMaintenanceInfoByFilter(MaintenanceMotorbikeFilterDto filter, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Motorbike> motorbikes, int totalCount) = await unitOfWork.MaintenanceRecordRepository.GetMotorbikesWithMaintenanceRecords(filter.PageNumber, filter.PageSize, filter.Search, filter.Status, cancellationToken);
            return new PaginatedDataDto<MaintenanceMotorbikeDto>(mapper.Map<IEnumerable<MaintenanceMotorbikeDto>>(motorbikes), totalCount);
        }
        public async Task<MaintenanceMotorbikeDto> GetMotorbikeMaintenanceInfoByMotorbikeId(int motorbikeId, CancellationToken cancellationToken = default)
        {
            Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetWithIncludes(m => m.MotorbikeId == motorbikeId,
                cancellationToken,
                m => m.MotorbikeMaintenanceInfo,
                m => m.Category) ?? throw new NotFoundException($"Motorbike with id {motorbikeId} not found");
            return mapper.Map<MaintenanceMotorbikeDto>(motorbike);
        }
    }
}
