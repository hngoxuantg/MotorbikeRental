using AutoMapper;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.DTOs.Vehicles;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;

namespace MotorbikeRental.Application.Services.VehicleServices
{
    public class MotorbikeService : IMotorbikeService
    {
        private readonly IMapper mapper;
        private readonly IMotorbikeValidator motorbikeValidator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        public MotorbikeService(IMapper mapper, IUnitOfWork unitOfWork, IMotorbikeValidator motorbikeValidator, IFileService fileService)
        {
            this.mapper = mapper;
            this.motorbikeValidator = motorbikeValidator;
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }

        public async Task<MotorbikeDto> CreateMotorbike(MotorbikeCreateDto motorbikeCreateDto, CancellationToken cancellationToken = default)
        {
            await motorbikeValidator.ValidateForCreate(motorbikeCreateDto, cancellationToken);
            Motorbike motorbike = mapper.Map<Motorbike>(motorbikeCreateDto);
            motorbike.Status = MotorbikeStatus.Available;
            motorbike.PriceList = new PriceList { HourlyRate = motorbikeCreateDto.HourlyRate, DailyRate = motorbikeCreateDto.DailyRate };
            if (motorbikeCreateDto.FormFile != null)
                motorbike.ImageUrl = await fileService.SaveImage(motorbikeCreateDto.FormFile, "Motorbike", cancellationToken);
            return mapper.Map<MotorbikeDto>(await unitOfWork.MotorbikeRepository.Create(motorbike));
        }

        public async Task<bool> DeleteMotorbike(int motorbikeId, CancellationToken cancellationToken = default)
        {
            Motorbike? motorbike = await unitOfWork.MotorbikeRepository.GetById(motorbikeId, cancellationToken);
            if (motorbike == null)
                throw new NotFoundException("MotorBike not found");
            motorbikeValidator.ValidateForDelete(motorbike);
            await unitOfWork.MotorbikeRepository.Delete(motorbike, cancellationToken);
            if (motorbike.ImageUrl != null)
                fileService.DeleteFile(motorbike.ImageUrl);
            return true;
        }

        public async Task<PaginatedDataDto<MotorbikeListDto>> GetMotorbikesByFilter(MotorbikeFilterDto motorbikeFilterDto, CancellationToken cancellationToken = default)
        {
            var (data, total) = await unitOfWork.MotorbikeRepository.GetFilterData(motorbikeFilterDto.CategoryId, motorbikeFilterDto.Brand, motorbikeFilterDto.Search, motorbikeFilterDto.Status, motorbikeFilterDto.PageNumber, motorbikeFilterDto.PageSize, cancellationToken);
            return new PaginatedDataDto<MotorbikeListDto>(mapper.Map<IEnumerable<MotorbikeListDto>>(data), total);
        }

        public async Task<MotorbikeDto> GetMotorbikeById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<MotorbikeDto>(await unitOfWork.MotorbikeRepository.GetByIdWithIncludes(id, cancellationToken));
        }

        public async Task<MotorbikeDto> UpdateMotorbike(MotorbikeUpdateDto motorbikeUpdateDto, CancellationToken cancellationToken = default)
        {
            await motorbikeValidator.ValidateForUpdate(motorbikeUpdateDto, cancellationToken);
            Motorbike motorbike = await unitOfWork.MotorbikeRepository.GetByIdWithIncludes(motorbikeUpdateDto.MotorbikeId, cancellationToken) ?? throw new Exception("Motorbike not found");
            if (motorbike.Status == MotorbikeStatus.Rented && motorbike.Status == MotorbikeStatus.Reserved)
                throw new BusinessRuleException("Motorbike is currently rented or reserved and cannot be updated.");
            motorbike.PriceList.DailyRate = motorbikeUpdateDto.DailyRate;
            motorbike.PriceList.HourlyRate = motorbikeUpdateDto.HourlyRate;
            if (motorbikeUpdateDto.FormFile != null)
            {
                if (motorbike.ImageUrl != null)
                    if (!fileService.DeleteFile(motorbike.ImageUrl)) throw new Exception("Failed to delete file");
                motorbike.ImageUrl = await fileService.SaveImage(motorbikeUpdateDto.FormFile, "Motorbike", cancellationToken);
            }
            motorbike.Category = null;
            await unitOfWork.MotorbikeRepository.Update(mapper.Map(motorbikeUpdateDto, motorbike), cancellationToken);
            MotorbikeDto result = mapper.Map<MotorbikeDto>(motorbike);
            result.CategoryName = await unitOfWork.CategoryRepository.GetCategoryNameById(motorbike.CategoryId, cancellationToken) ?? throw new NotFoundException("Category not found");
            return result;
        }
        public async Task<MotorbikeIndexDto> GetMotorbikeFilterOptions(CancellationToken cancellationToken = default)
        {
            return new MotorbikeIndexDto
            {
                CategoriesDto = mapper.Map<IEnumerable<CategoryDto>>(await unitOfWork.CategoryRepository.GetCategoriesNoTracking(cancellationToken)),
                Brands = await unitOfWork.MotorbikeRepository.GetDistinctBrands(cancellationToken)
            };
        }
    }
}