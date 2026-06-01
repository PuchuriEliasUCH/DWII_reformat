using AltaMesa.web.DTOs;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginDTO> Login(string correo, string password);
    }
}