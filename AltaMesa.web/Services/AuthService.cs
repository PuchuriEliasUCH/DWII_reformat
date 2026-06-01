using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<LoginDTO> Login(string correo, string password)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreo(correo).ConfigureAwait(false);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, usuario.ContraHash))
                return null;

            usuario.ContraHash = null;
            return usuario;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
