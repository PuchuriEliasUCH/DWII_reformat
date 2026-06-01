using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task Crear(CrearUsuarioDTO dto);
        Task<List<UsuarioDTO>> Listar();
        Task Actualizar(ActualizarUsuarioDTO dto);
        Task Eliminar(int id);
    }
}