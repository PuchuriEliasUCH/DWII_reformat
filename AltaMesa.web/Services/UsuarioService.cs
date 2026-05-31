using System.Collections.Generic;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        public void Crear(CrearUsuarioDTO dto)
        {
            dto.Password = AuthService.HashPassword(dto.Password);
            _usuarioRepository.Crear(dto);
        }

        public List<UsuarioDTO> Listar()
        {
            return _usuarioRepository.Listar();
        }

        public void Actualizar(ActualizarUsuarioDTO dto)
        {
            _usuarioRepository.Actualizar(dto);
        }

        public void Eliminar(int id)
        {
            _usuarioRepository.Eliminar(id);
        }
    }
}
