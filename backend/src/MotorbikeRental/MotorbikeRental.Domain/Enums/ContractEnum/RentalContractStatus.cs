namespace MotorbikeRental.Domain.Enums.ContractEnum
{
    public enum RentalContractStatus
    {
        Active = 0,      // Đang thuê
        Completed = 1,   // Đã hoàn thành
        Cancelled = 2,   // Đã hủy
        Pending = 3,     // Đang chờ xử lý
        ProcessingIncident = 4, // Đang xử lý sự cố
    }
}