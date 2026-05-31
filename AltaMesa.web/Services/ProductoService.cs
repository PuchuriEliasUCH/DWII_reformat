using System.Collections.Generic;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class ProductoService
    {
        private readonly ProductoRepository _productoRepository;

        public ProductoService()
        {
            _productoRepository = new ProductoRepository();
        }

        public void Crear(CrearProductoDTO dto)
        {
            _productoRepository.Crear(dto);
        }

        public List<ProductoDTO> Listar()
        {
            return _productoRepository.Listar();
        }
    }
}
