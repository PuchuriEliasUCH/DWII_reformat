using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task Crear(CrearCategoriaDTO dto);
        Task Actualizar(ActualizarCategoriaDTO dto);
        Task Desactivar(int id);
        Task<List<CategoriaDTO>> Listar();
    }
}