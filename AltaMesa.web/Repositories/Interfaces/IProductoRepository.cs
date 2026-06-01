using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task Crear(CrearProductoDTO dto);
        Task<List<ProductoDTO>> Listar();
    }
}