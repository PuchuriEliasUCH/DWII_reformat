using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<LoginDTO> ObtenerPorCorreo(string correo);
        Task Crear(CrearUsuarioDTO dto);
        Task<List<UsuarioDTO>> Listar();
        Task Actualizar(ActualizarUsuarioDTO dto);
        Task Eliminar(int id);
    }
}