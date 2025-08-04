using MotorbikeRental.Application.DTOs.Emails;

namespace MotorbikeRental.Application.Interface.IExternalServices.IMailServices
{
    public interface IMailService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
