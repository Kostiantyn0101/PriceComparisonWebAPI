namespace Domain.Models.Response.Auth
{
    public class RefreshTokenResponseModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
