namespace Domain.Models.Auth
{
    public class RefreshTokenResponseModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
