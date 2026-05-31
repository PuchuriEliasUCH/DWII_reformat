using System.Collections.Generic;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class CategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaService()
        {
            _categoriaRepository = new CategoriaRepository();
        }

        public void Crear(CrearCategoriaDTO dto)
        {
            _categoriaRepository.Crear(dto);
        }

        public void Actualizar(ActualizarCategoriaDTO dto)
        {
            _categoriaRepository.Actualizar(dto);
        }

        public void Desactivar(int id)
        {
            _categoriaRepository.Desactivar(id);
        }

        public List<CategoriaDTO> Listar()
        {
            return _categoriaRepository.Listar();
        }
    }
}
