using MotorbikeRental.Application.Interface.IDataSeedingServices;
using MotorbikeRental.Application.Interface.IExternalServices.IMailServices;
using MotorbikeRental.Application.Interface.IExternalServices.Storage;
using MotorbikeRental.Application.Interface.IServices.IAuthServices;
using MotorbikeRental.Application.Interface.IServices.IContractServices;
using MotorbikeRental.Application.Interface.IServices.ICustomerServices;
using MotorbikeRental.Application.Interface.IServices.IDiscountServices;
using MotorbikeRental.Application.Interface.IServices.IIncidentServices;
using MotorbikeRental.Application.Interface.IServices.IStatisticsServices;
using MotorbikeRental.Application.Interface.IServices.IUserServices;
using MotorbikeRental.Application.Interface.IServices.IVehicleServices;
using MotorbikeRental.Application.Interface.IValidators.IContractValidators;
using MotorbikeRental.Application.Interface.IValidators.ICustomerValidators;
using MotorbikeRental.Application.Interface.IValidators.IDiscountValidators;
using MotorbikeRental.Application.Interface.IValidators.IIncidentValidators;
using MotorbikeRental.Application.Interface.IValidators.IUserValidators;
using MotorbikeRental.Application.Interface.IValidators.IVehicleValidators;
using MotorbikeRental.Application.Mappers;
using MotorbikeRental.Application.Services.AuthServices;
using MotorbikeRental.Application.Services.ContractServices;
using MotorbikeRental.Application.Services.CustomerServices;
using MotorbikeRental.Application.Services.DiscountServices;
using MotorbikeRental.Application.Services.IncidentServices;
using MotorbikeRental.Application.Services.StatisticsServices;
using MotorbikeRental.Application.Services.UserServices;
using MotorbikeRental.Application.Services.VehicleServices;
using MotorbikeRental.Application.Validators.ContractValidators;
using MotorbikeRental.Application.Validators.CustomerValidators;
using MotorbikeRental.Application.Validators.DiscountValidators;
using MotorbikeRental.Application.Validators.IncidentValidators;
using MotorbikeRental.Application.Validators.UserValidators;
using MotorbikeRental.Application.Validators.VehicleValidators;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.ICustomerRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IIncidents;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IStatisticsRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IUserRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories.IVehicleRepositories;
using MotorbikeRental.Infrastructure.Data.DataSeedingServices;
using MotorbikeRental.Infrastructure.Data.Repositories.ContractRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories.CustomerRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories.IncidentRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories.PricingRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories.UserRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories.VehicleRepositories;
using MotorbikeRental.Infrastructure.ExternalServices.MailService;
using MotorbikeRental.Infrastructure.ExternalServices.StorageService;
using MotorbikeRental.Infrastructure.Data.Repositories.StatisticsRepositories;
using MotorbikeRental.Domain.Interfaces.IRepositories;
using MotorbikeRental.Infrastructure.Data.Repositories;

namespace MotorbikeRental.Web.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            RegisterServices(services);
            RegisterRepositories(services);
            RegisterExternalService(services);
            RegisterDataSeedingService(services);
            RegisterAutoMapper(services);
            RegisterValidator(services);
            return services;
        }
        private static IServiceCollection RegisterServices(IServiceCollection services)
        {
            //UserServices
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserCredentialsService, UserCredentialsService>();
            //VehicleServices
            services.AddScoped<IMotorbikeService, MotorbikeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMaintenanceRecordService, MaintenanceRecordService>();
            //CustomerServicesd
            services.AddScoped<ICustomerService, CustomerService>();
            //AuthServices
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            //ContractServices
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IPaymentService, PaymentService>();
            //DiscountServices
            services.AddScoped<IDiscountService, DiscountService>();
            //IncidentServices
            services.AddScoped<IIncidentService, IncidentService>();
            //StatisticsServices
            services.AddScoped<IStatisticsService, StatisticsService>();
            return services;
        }
        private static IServiceCollection RegisterRepositories(IServiceCollection services)
        {
            //VehicleRepository
            services.AddScoped<IMotorbikeRepository, MotorbikeRepository>();
            services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //UserRepository
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
            //ContractRepository
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            //IncidentsRepository
            services.AddScoped<IIncidentRepository, IncidentRepository>();
            services.AddScoped<IIncidentImageRepository, IncidentImageRepository>();
            //CustomerRepository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            //PricingRepository
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IPriceListRepository, PriceListRepository>();
            //StatisticsRepository
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        private static IServiceCollection RegisterExternalService(IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMailService, MailService>();
            return services;
        }
        private static IServiceCollection RegisterDataSeedingService(IServiceCollection services)
        {
            services.AddScoped<IDataSeedingService, DataSeedingService>();
            return services;
        }
        private static IServiceCollection RegisterAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RoleProfile).Assembly);
            return services;
        }
        private static IServiceCollection RegisterValidator(IServiceCollection services)
        {
            //VehicleValidator
            services.AddScoped<IMotorbikeValidator, MotorbikeValidator>();
            services.AddScoped<ICategoryValidator, CategoryValidator>();
            services.AddScoped<IMaintenanceRecordValidator, MaintenanceRecordValidator>();
            //CustomerValidator
            services.AddScoped<ICustomerValidator, CustomerValidator>();
            //EmployeeValidator
            services.AddScoped<IEmployeeValidator, EmployeeValidator>();
            services.AddScoped<IUserCredentialsValidator, UserCredentialsValidator>();
            //DiscountValidator
            services.AddScoped<IDiscountValidator, DiscountValidator>();
            //ContractValidator
            services.AddScoped<IContractValidator, ContractValidator>();
            //IncidentValidator
            services.AddScoped<IIncidentValidator, IncidentValidator>();
            //PaymentValidator
            services.AddScoped<IPaymentValidator, PaymentValidator>();
            return services;
        }
    }
}