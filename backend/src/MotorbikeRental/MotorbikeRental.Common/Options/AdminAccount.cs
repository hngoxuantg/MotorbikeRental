namespace MotorbikeRental.Common.Options
{
    public class AdminAccount
    {
        public Info? Info { get; set; }
        public Account? Account { get; set; }
    }
    public class Info
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
    public class Account
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
