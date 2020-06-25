using Core.Models.Users.DTOs;

namespace Core.Models.Login.DTOs
{
    public class LoginOutputDTO
    {
        public string AccessToken { get; set; }

        public UserOutputDTO User { get; set; }
    }
}
