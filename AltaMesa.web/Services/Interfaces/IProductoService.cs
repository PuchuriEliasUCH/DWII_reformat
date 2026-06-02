using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IProductoService
    {
        Task Crear(CrearProductoDTO dto);
        Task<List<ProductoDTO>> Listar();
        Task<ProductoDTO> ObtenerPorId(int id);
        Task Actualizar(ActualizarProductoDTO dto);
        Task Eliminar(int id);
    }
}