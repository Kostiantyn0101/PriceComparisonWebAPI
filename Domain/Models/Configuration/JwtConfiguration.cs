namespace Domain.Models.Configuration
{
    public class JwtConfiguration
    {
        public const string Position = "JWT";
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public string? Key { get; set; }
        public double AccessTokenLifetimeMin { get; set; }
        public double DefaultRefreshTokenLifetimeHours { get; set; }
        public double RememberMeRefreshTokenLifetimeHours { get; set; }
    }
}
