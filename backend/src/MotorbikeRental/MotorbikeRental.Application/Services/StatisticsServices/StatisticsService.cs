using MotorbikeRental.Application.DTOs.Statistics;
using MotorbikeRental.Application.Exceptions;
using MotorbikeRental.Application.Interface.IServices.IStatisticsServices;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;
using MotorbikeRental.Domain.Enums.VehicleEnum;
using MotorbikeRental.Domain.Interfaces.IRepositories;

namespace MotorbikeRental.Application.Services.StatisticsServices
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork unitOfWork;
        public StatisticsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<OverviewStatisticsDto> GetOverview(CancellationToken cancellationToken = default)
        {
            DateTime systemStartDate = await unitOfWork.StatisticsRepository.GetSystemStartDate(cancellationToken);

            return new OverviewStatisticsDto
            {
                TotalMotorbikes = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(null, cancellationToken),
                TotalRentals = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(r => r.RentalContractStatus == RentalContractStatus.Completed, cancellationToken),
                TotalCustomers = await unitOfWork.StatisticsRepository.GetTotalCount<Customer>(null, cancellationToken),
                TotalRevenue = await unitOfWork.StatisticsRepository.GetTotalPrice<Payment>(p => p.Amount, null, cancellationToken),
                TotalSalary = CalculateTotalSalary(await unitOfWork.StatisticsRepository.GetSalaries(cancellationToken)),
                TotalMaintenanceCost = await unitOfWork.StatisticsRepository.GetTotalPrice<MaintenanceRecord>(m => m.Cost ?? 0, null, cancellationToken),
                TotalIncidents = await unitOfWork.StatisticsRepository.GetTotalCount<Incident>(null, cancellationToken),
                TotalEmployees = await unitOfWork.StatisticsRepository.GetTotalCount<Employee>(null, cancellationToken),
                SystemStartDate = systemStartDate == null ? DateTime.MinValue : systemStartDate
            };
        }

        public async Task<MotorbikeStatusStatisticsDto> GetMotorbikeStatusStatistics(CancellationToken cancellationToken = default)
        {
            return new MotorbikeStatusStatisticsDto
            {
                Available = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.Available, cancellationToken),
                Rented = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.Rented, cancellationToken),
                UnderMaintenance = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.UnderMaintenance, cancellationToken),
                Reserved = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.Reserved, cancellationToken),
                OutOfService = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.OutOfService, cancellationToken),
                Damaged = await unitOfWork.StatisticsRepository.GetTotalCount<Motorbike>(m => m.Status == MotorbikeStatus.Damaged, cancellationToken)
            };
        }
        public async Task<ContractStatusStatisticsDto> GetContractStatusStatistics(CancellationToken cancellationToken = default)
        {
            return new ContractStatusStatisticsDto
            {
                TotalContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(null, cancellationToken),

                TotalActiveContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(
                    c => c.RentalContractStatus == RentalContractStatus.Active, 
                    cancellationToken),

                TotalCompletedContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(
                    c => c.RentalContractStatus == RentalContractStatus.Completed, 
                    cancellationToken),

                TotalCancelledContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(
                    c => c.RentalContractStatus == RentalContractStatus.Cancelled, 
                    cancellationToken),

                TotalPendingContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(
                    c => c.RentalContractStatus == RentalContractStatus.Pending, 
                    cancellationToken),

                TotalProcessingContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(
                    c => c.RentalContractStatus == RentalContractStatus.ProcessingIncident, 
                    cancellationToken)
            };
        }
        public async Task<IncidentStatisticsDto> GetIncidentStatistics(CancellationToken cancellationToken = default)
        {
            return new IncidentStatisticsDto
            {
                TotalIncidents = await unitOfWork.StatisticsRepository.GetTotalCount<Incident>(null, cancellationToken),
                ResolvedIncidents = await unitOfWork.StatisticsRepository.GetTotalCount<Incident>(i => i.IsResolved, cancellationToken),
                UnresolvedIncidents = await unitOfWork.StatisticsRepository.GetTotalCount<Incident>(i => !i.IsResolved, cancellationToken)
            };
        }
        public async Task<RevenueStatisticsDto> GetRevenueByDateRange(StatisticsFilterDto filter, CancellationToken cancellation = default)
        {
            DateTime systemStar = await unitOfWork.StatisticsRepository.GetSystemStartDate(cancellation);

            if (filter.FromDate < systemStar || !filter.FromDate.HasValue)
                filter.FromDate = systemStar;
            if (!filter.ToDate.HasValue || filter.ToDate < filter.FromDate)
                filter.ToDate = DateTime.UtcNow.Date;

            (IEnumerable<Payment> payments, IEnumerable<MaintenanceRecord> maintenanceRecords) = await unitOfWork.StatisticsRepository.GetPaymentsAndMaintenanceRecords(
                filter.FromDate.Value, 
                filter.ToDate.Value, 
                cancellation);

            return await AssignRevenueBreakdown(filter, payments, maintenanceRecords, cancellation);
        }
        private async Task<RevenueStatisticsDto> AssignRevenueBreakdown(StatisticsFilterDto filter, IEnumerable<Payment> payments, IEnumerable<MaintenanceRecord> maintenanceRecords, CancellationToken cancellationToken = default)
        {
            RevenueStatisticsDto revenueStatisticsDto = new RevenueStatisticsDto
            {
                ToDate = filter.FromDate,
                EndDate = filter.ToDate,
                TotalRevenue = payments.Sum(p => p.Amount),
                TotalMaintenanceCost = maintenanceRecords.Sum(m => m.Cost ?? 0),
                TotalContracts = await unitOfWork.StatisticsRepository.GetTotalCount<RentalContract>(null, cancellationToken),
            };

            TimeSpan totalDate = filter.ToDate.Value - filter.FromDate.Value;

            if (totalDate.Days <= 60)
            {
                revenueStatisticsDto.DailyRevenues = MapStatisticsToDailyRevenue(payments, maintenanceRecords, filter.FromDate.Value, filter.ToDate);
            }
            if (totalDate.Days > 60 && totalDate.Days <= 365)
            {
                revenueStatisticsDto.MonthlyRevenues = MapStatisticsToMonthlyRevenue(payments, maintenanceRecords, filter.FromDate.Value, filter.ToDate);
            }
            if (totalDate.Days > 365)
            {
                revenueStatisticsDto.YearlyRevenues = MapStatisticsToYearlyRevenue(payments, maintenanceRecords, filter.FromDate.Value, filter.ToDate);
            }

            return revenueStatisticsDto;
        }
        public async Task<MonthlyHighlightDto> GetMonthlyHighlights(HighlightFilterDto filter, CancellationToken cancellationToken = default)
        {
            if (filter.Year < (await unitOfWork.StatisticsRepository.GetSystemStartDate(cancellationToken)).Year)
                throw new BusinessRuleException("Year must be greater than or equal to the system start year.");
            if (filter.Year > DateTime.UtcNow.Year)
                throw new ValidatorException("Year must be less than or equal to the current year.");

            (IEnumerable<Employee> employees, IEnumerable<Motorbike> motorbikes) = await unitOfWork.StatisticsRepository.GetEmployeesAndMotorbikesHighlight(filter.Month, filter.Year, cancellationToken);
            (IEnumerable<TopEmployeeOfMonthDto> topEmployees, IEnumerable<TopRentedMotorbikeDto> topMotorbikes) = MapEmployeesAndMotorbikesHighlight(employees.ToList(), motorbikes.ToList());

            MonthlyHighlightDto monthlyHighlightDto = new MonthlyHighlightDto
            {
                Month = filter.Month,
                Year = filter.Year,
                TopEmployees = topEmployees,
                TopMotorbikes = topMotorbikes
            };

            return monthlyHighlightDto;
        }
        private (IEnumerable<TopEmployeeOfMonthDto>, IEnumerable<TopRentedMotorbikeDto>) MapEmployeesAndMotorbikesHighlight(List<Employee> employees, List<Motorbike> motorbikes)
        {
            List<TopEmployeeOfMonthDto> topEmployees = new List<TopEmployeeOfMonthDto>();
            List<TopRentedMotorbikeDto> topMotorbikes = new List<TopRentedMotorbikeDto>();

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].RentalContracts.Count == 0)
                    continue;

                topEmployees.Add(new TopEmployeeOfMonthDto
                {
                    EmployeeId = employees[i].EmployeeId,
                    EmployeeName = employees[i].FullName,
                    Avatar = employees[i].Avatar,
                    ContractCount = employees[i].RentalContracts.Count
                });
            }
            for (int i = 0; i < motorbikes.Count; i++)
            {
                if (motorbikes[i].RentalContracts.Count == 0)
                    continue;

                topMotorbikes.Add(new TopRentedMotorbikeDto
                {
                    MotorbikeId = motorbikes[i].MotorbikeId,
                    MotorbikeName = motorbikes[i].MotorbikeName,
                    MotorbikeImage = motorbikes[i].ImageUrl,
                    RentalCount = motorbikes[i].RentalContracts.Count
                });
            }

            return (topEmployees, topMotorbikes);
        }
        private IEnumerable<DailyRevenueDto> MapStatisticsToDailyRevenue(IEnumerable<Payment> payments, IEnumerable<MaintenanceRecord> maintenanceRecords, DateTime toDate, DateTime? endDate)
        {
            DateTime startDate = toDate.Date;
            DateTime endDateValue = endDate?.Date ?? DateTime.UtcNow.Date;

            List<DailyRevenueDto> dailyRevenues = new List<DailyRevenueDto>();

            for (DateTime date = startDate; date <= endDateValue; date = date.AddDays(1))
            {
                decimal totalRevenue = payments.Where(p => p.PaymentDate.Date == date).Sum(p => p.Amount);
                decimal totalMaintenanceCost = maintenanceRecords.Where(m => m.MaintenanceDate.Date == date).Sum(m => m.Cost ?? 0);

                dailyRevenues.Add(new DailyRevenueDto
                {
                    Date = date,
                    TotalRevenue = totalRevenue,
                    TotalMaintenanceCost = totalMaintenanceCost
                });
            }

            return dailyRevenues;
        }
        private IEnumerable<MonthlyRevenueDto> MapStatisticsToMonthlyRevenue(IEnumerable<Payment> payments, IEnumerable<MaintenanceRecord> maintenanceRecords, DateTime toDate, DateTime? endDate)
        {
            DateTime starDate = toDate.Date;
            DateTime endDateValue = endDate?.Date ?? DateTime.UtcNow.Date;

            List<MonthlyRevenueDto> monthRevenueDto = new List<MonthlyRevenueDto>();

            for (DateTime date = starDate; date <= endDateValue.Date; date = date.AddMonths(1))
            {
                decimal totalRevenue = payments.Where(p => p.PaymentDate.Year == date.Year && p.PaymentDate.Month == date.Month).Sum(p => p.Amount);
                decimal totalMaintenanceCost = maintenanceRecords.Where(m => m.MaintenanceDate.Year == date.Year && m.MaintenanceDate.Month == date.Month).Sum(m => m.Cost ?? 0);

                monthRevenueDto.Add(new MonthlyRevenueDto
                {
                    Year = date.Year,
                    Monthly = date.Month,
                    TotalRevenue = totalRevenue,
                    TotalMaintenanceCost = totalMaintenanceCost
                });
            }

            return monthRevenueDto;
        }
        private IEnumerable<YearlyRevenueDto> MapStatisticsToYearlyRevenue(IEnumerable<Payment> payments, IEnumerable<MaintenanceRecord> maintenanceRecords, DateTime toDate, DateTime? endDate)
        {
            DateTime starDate = toDate.Date;
            DateTime endDateValue = endDate?.Date ?? DateTime.UtcNow.Date;

            List<YearlyRevenueDto> yearlyRevenueDtos = new List<YearlyRevenueDto>();

            for (DateTime date = starDate; date <= endDateValue.Date; date = date.AddYears(1))
            {
                decimal totalRevenue = payments.Where(p => p.PaymentDate.Year == date.Year).Sum(p => p.Amount);
                decimal totalMaintenanceCost = maintenanceRecords.Where(m => m.MaintenanceDate.Year == date.Year).Sum(m => m.Cost ?? 0);

                yearlyRevenueDtos.Add(new YearlyRevenueDto
                {
                    Year = date.Year,
                    TotalRevenue = totalRevenue,
                    TotalMaintenanceCost = totalMaintenanceCost
                });
            }
            return yearlyRevenueDtos;
        }
        private decimal CalculateTotalSalary(IEnumerable<Employee> employees)
        {
            decimal totalSalary = 0;

            List<Employee> employeeList = employees.ToList();

            for (int i = 0; i < employees.Count(); i++)
            {
                int totalMonths = ((DateTime.UtcNow.Year - employeeList[i].StartDate.Year) * 12) + (DateTime.UtcNow.Month - employeeList[i].StartDate.Month);
                totalSalary += (decimal)(totalMonths * employeeList[i].Salary);
            }
            return totalSalary;
        }
    }
}
