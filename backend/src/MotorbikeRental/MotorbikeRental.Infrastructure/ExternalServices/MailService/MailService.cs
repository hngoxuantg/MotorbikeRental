using Microsoft.Extensions.Configuration;
using MotorbikeRental.Application.DTOs.Emails;
using MotorbikeRental.Application.Interface.IExternalServices.IMailServices;
using System.Net;
using System.Net.Mail;

namespace MotorbikeRental.Infrastructure.ExternalServices.MailService
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;
        private readonly string password;
        private readonly string fromEmail;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            password = configuration["EmailSettings:Password"];
            fromEmail = configuration["EmailSettings:From"];
        }
        public async Task SendEmail(EmailDto emailDto)
        {
            MailAddress mailAddress = new MailAddress(fromEmail);
            MailAddress toAddress = new MailAddress(emailDto.To);
            SmtpClient smptClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailAddress.Address, password),
                Timeout = 20000
            };
            using var message = new MailMessage(mailAddress, toAddress)
            {
                Subject = "Thông báo tài khoản nhân viên",
                Body = BodyTemplate(emailDto),
                IsBodyHtml = true
            };

            await smptClient.SendMailAsync(message);
        }
        public string BodyTemplate(EmailDto emailDto)
        {
            string body = $@"
    <html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
        }}
        .container {{
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
            width: 100%;
            max-width: 600px;
            margin: auto;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #777;
        }}
        .login-button {{
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            background-color: #2a7ae2;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Thông báo tài khoản nhân viên</h2>
        <p>Chào <strong>{emailDto.To}</strong>,</p>

        <p>Bạn đã được tạo tài khoản để truy cập vào hệ thống nội bộ của công ty. Dưới đây là thông tin đăng nhập của bạn:</p>

        <ul>
            <li><strong>Email:</strong> {emailDto.UserName}</li>
            <li><strong>Mật khẩu:</strong> {emailDto.Password}</li>
        </ul>

        <p>Vui lòng đăng nhập và thay đổi mật khẩu ngay sau lần đăng nhập đầu tiên vì lý do bảo mật.</p>

        <a href='http://localhost:5174/' class='login-button'>Đăng nhập hệ thống</a>

        <p>Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ bộ phận IT để được hỗ trợ.</p>

        <p>Trân trọng,<br/>Phòng Hành Chính - Nhân Sự</p>

        <div class='footer'>
            Email này được gửi tự động. Vui lòng không trả lời lại.
        </div>
    </div>
</body>
</html>
";
            return body;
        }
    }
}
