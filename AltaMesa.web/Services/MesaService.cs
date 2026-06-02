using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class MesaService : IMesaService
    {
        private readonly IMesaRepository _mesaRepository;

        public MesaService(IMesaRepository mesaRepository)
        {
            _mesaRepository = mesaRepository;
        }

        public async Task Crear(CrearMesaDTO dto)
        {
            await _mesaRepository.Crear(dto).ConfigureAwait(false);
        }

        public async Task<List<MesaDTO>> Listar()
        {
            return await _mesaRepository.Listar().ConfigureAwait(false);
        }

        public async Task<MesaDTO> ObtenerPorId(int id)
        {
            return await _mesaRepository.ObtenerPorId(id).ConfigureAwait(false);
        }

        public async Task Actualizar(ActualizarMesaDTO dto)
        {
            await _mesaRepository.Actualizar(dto).ConfigureAwait(false);
        }
    }
}
