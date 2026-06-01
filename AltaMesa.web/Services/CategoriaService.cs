using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task Crear(CrearCategoriaDTO dto)
        {
            await _categoriaRepository.Crear(dto).ConfigureAwait(false);
        }

        public async Task Actualizar(ActualizarCategoriaDTO dto)
        {
            await _categoriaRepository.Actualizar(dto).ConfigureAwait(false);
        }

        public async Task Desactivar(int id)
        {
            await _categoriaRepository.Desactivar(id).ConfigureAwait(false);
        }

        public async Task<List<CategoriaDTO>> Listar()
        {
            return await _categoriaRepository.Listar().ConfigureAwait(false);
        }
    }
}
