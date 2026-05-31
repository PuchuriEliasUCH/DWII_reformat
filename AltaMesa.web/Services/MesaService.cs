using System.Collections.Generic;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class MesaService
    {
        private readonly MesaRepository _mesaRepository;

        public MesaService()
        {
            _mesaRepository = new MesaRepository();
        }

        public void Crear(CrearMesaDTO dto)
        {
            _mesaRepository.Crear(dto);
        }

        public List<MesaDTO> Listar()
        {
            return _mesaRepository.Listar();
        }

        public void Actualizar(ActualizarMesaDTO dto)
        {
            _mesaRepository.Actualizar(dto);
        }
    }
}
