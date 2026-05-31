using System;
using System.Security.Cryptography;
using System.Text;
using AltaMesa.web.DTOs;
using AltaMesa.web.Helpers;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public AuthService()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        public LoginDTO Login(string correo, string password)
        {
            var hash = HashPassword(password);
            var usuario = _usuarioRepository.Login(correo, hash);

            if (usuario != null)
            {
                SessionHelper.SetSession(
                    usuario.IdUsuario,
                    usuario.NombreUsuario,
                    usuario.NombreRol,
                    usuario.CorreoUsuario
                );
            }

            return usuario;
        }

        public void Logout()
        {
            SessionHelper.DestroySession();
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
