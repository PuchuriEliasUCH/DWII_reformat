using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task Crear(CrearUsuarioDTO dto)
        {
            dto.Password = AuthService.HashPassword(dto.Password);
            await _usuarioRepository.Crear(dto).ConfigureAwait(false);
        }

        public async Task<List<UsuarioDTO>> Listar()
        {
            return await _usuarioRepository.Listar().ConfigureAwait(false);
        }

        public async Task Actualizar(ActualizarUsuarioDTO dto)
        {
            await _usuarioRepository.Actualizar(dto).ConfigureAwait(false);
        }

        public async Task Eliminar(int id)
        {
            await _usuarioRepository.Eliminar(id).ConfigureAwait(false);
        }
    }
}
