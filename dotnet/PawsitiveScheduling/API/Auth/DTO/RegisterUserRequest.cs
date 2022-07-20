namespace PawsitiveScheduling.API.Auth.DTO
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string UserRole { get; set; }
    }
}
