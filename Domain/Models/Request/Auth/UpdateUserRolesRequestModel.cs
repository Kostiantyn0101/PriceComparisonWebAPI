namespace Domain.Models.Request.Auth
{
    public class UpdateUserRolesRequestModel
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
