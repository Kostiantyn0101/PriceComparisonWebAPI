namespace Domain.Models.Request.Auth
{
    public class LoginRequestModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
