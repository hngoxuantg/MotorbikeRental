using AutoMapper;
using MotorbikeRental.Application.DTOs.MaintenanceRecord;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Application.Mappers
{
    public class MaintenanceRecordProfile : Profile
    {
        public MaintenanceRecordProfile()
        {
            CreateMap<MaintenanceRecordCreateDto, MaintenanceRecord>();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>()
                .ForMember(dest => dest.MotorbikeName,
                opt => opt.MapFrom(src => src.Motorbike.MotorbikeName))
                .ForMember(dest => dest.MotorbikeImage,
                opt => opt.MapFrom(src => src.Motorbike.ImageUrl))
                .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.FullName));
            CreateMap<MaintenanceCompletionDto, MaintenanceRecord>();
            CreateMap<Motorbike, MaintenanceMotorbikeDto>()
                .ForMember(dest => dest.LastMaintenanceDate,
                opt => opt.MapFrom(src => src.MotorbikeMaintenanceInfo.LastMaintenanceDate))
                .ForMember(dest => dest.NextMaintenanceDate,
                opt => opt.MapFrom(src => src.MotorbikeMaintenanceInfo.NextMaintenanceDate))
                .ForMember(dest => dest.MaintenanceCount,
                opt => opt.MapFrom(src => src.MotorbikeMaintenanceInfo.MaintenanceCount))
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.DaysUntilNextMaintenance,
                opt => opt.MapFrom(src =>
                src.MotorbikeMaintenanceInfo.NextMaintenanceDate.HasValue
                    ? (int?)(src.MotorbikeMaintenanceInfo.NextMaintenanceDate.Value.Date - DateTime.Now.Date).TotalDays
                    : null));
        }
    }
}