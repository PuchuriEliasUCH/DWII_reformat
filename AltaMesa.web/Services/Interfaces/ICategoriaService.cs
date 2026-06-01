using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task Crear(CrearCategoriaDTO dto);
        Task Actualizar(ActualizarCategoriaDTO dto);
        Task Desactivar(int id);
        Task<List<CategoriaDTO>> Listar();
    }
}