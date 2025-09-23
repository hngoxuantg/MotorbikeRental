using AutoMapper;
using MotorbikeRental.Application.DTOs.Discount;
using MotorbikeRental.Application.DTOs.Pagination;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IDiscountServices;
using MotorbikeRental.Application.Interface.IValidators.IDiscountValidators;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountValidator discountValidator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public DiscountService(IDiscountValidator discountValidator, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.discountValidator = discountValidator;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DiscountDto> CreateDiscount(DiscountCreateDto discountCreateDto)
        {
            (bool isValid, List<string> categoryName) = await discountValidator.ValidateForCreate(discountCreateDto);

            Discount discount = mapper.Map<Discount>(discountCreateDto);

            List<Discount_Category> discountCategories = new List<Discount_Category>();
            for (int i = 0; i < discountCreateDto.CategoryId.Count; i++)
            {
                discountCategories.Add(new Discount_Category
                {
                    CategoryId = discountCreateDto.CategoryId[i],
                });
            }

            discount.Categories = discountCategories;
            discount.CreatedAt = DateTime.UtcNow;

            await unitOfWork.DiscountRepository.Create(discount);

            DiscountDto discountDto = mapper.Map<DiscountDto>(discount);

            discountDto.CategoryNames = categoryName;

            return discountDto;
        }
        public async Task<PaginatedDataDto<DiscountDto>> GetDiscountsByFilter(DiscountFilterDto filter, CancellationToken cancellationToken = default)
        {
            (IEnumerable<Discount> discount, int totalCount) = await unitOfWork.DiscountRepository.GetFilterData(
                filter.Search, 
                filter.PageNumber, 
                filter.PageSize, 
                filter.CategoryId, 
                filter.FilterStartDate, 
                filter.FilterEndDate, 
                filter.IsActive, 
                cancellationToken);

            return new PaginatedDataDto<DiscountDto>(mapper.Map<IEnumerable<DiscountDto>>(discount), totalCount);
        }
        public async Task<DiscountDto> GetDiscountById(int id, CancellationToken cancellationToken = default)
        {
            return mapper.Map<DiscountDto>(await unitOfWork.DiscountRepository.GetDiscountById(id, false, cancellationToken) 
                ?? throw new NotFoundException($"Discount with id {id} not found"));
        }
        public async Task<DiscountDto> UpdateDiscount(DiscountUpdateDto discountUpdateDto, CancellationToken cancellationToken = default)
        {
            (bool isValid, List<string> categoryNames) = await discountValidator.ValidateForUpdate(discountUpdateDto, cancellationToken);

            Discount discount = await unitOfWork.DiscountRepository.GetDiscountById(discountUpdateDto.DiscountId, true, cancellationToken) ?? throw new NotFoundException($"Discount with id {discountUpdateDto.DiscountId} not found");

            mapper.Map(discountUpdateDto, discount);

            discount.Categories.Clear();

            for (int i = 0; i < discountUpdateDto.CategoryId.Count; i++)
            {
                discount.Categories.Add(new Discount_Category
                {
                    CategoryId = discountUpdateDto.CategoryId[i],
                });
            }

            await unitOfWork.DiscountRepository.Update(discount, cancellationToken);

            DiscountDto discountDto = mapper.Map<DiscountDto>(discount);

            discountDto.CategoryNames = categoryNames;

            return discountDto;
        }
        public async Task<bool> DeleteDiscount(int id, CancellationToken cancellationToken = default)
        {
            Discount? discount = await unitOfWork.DiscountRepository.GetById(id, cancellationToken) 
                ?? throw new NotFoundException($"Discount with id {id} not found");

            await unitOfWork.DiscountRepository.Delete(discount, cancellationToken);

            return true;
        }

    }
}
