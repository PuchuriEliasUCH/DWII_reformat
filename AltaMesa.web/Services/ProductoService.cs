using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task Crear(CrearProductoDTO dto)
        {
            await _productoRepository.Crear(dto).ConfigureAwait(false);
        }

        public async Task<List<ProductoDTO>> Listar()
        {
            return await _productoRepository.Listar().ConfigureAwait(false);
        }

        public async Task<ProductoDTO> ObtenerPorId(int id)
        {
            return await _productoRepository.ObtenerPorId(id).ConfigureAwait(false);
        }

        public async Task Actualizar(ActualizarProductoDTO dto)
        {
            await _productoRepository.Actualizar(dto).ConfigureAwait(false);
        }

        public async Task Eliminar(int id)
        {
            await _productoRepository.Eliminar(id).ConfigureAwait(false);
        }
    }
}
