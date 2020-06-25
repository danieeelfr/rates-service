using Core.Filters.Login;
using Core.Models.Login.DTOs;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ILoginService
    {
        Task<LoginOutputDTO> Login(UserLoginFilter filter, string secret);
    }
}