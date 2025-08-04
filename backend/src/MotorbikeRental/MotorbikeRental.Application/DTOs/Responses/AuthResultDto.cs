namespace MotorbikeRental.Application.DTOs.Responses
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string AccessToken { get; set; }
    }
}
