using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IMesaService
    {
        Task Crear(CrearMesaDTO dto);
        Task<List<MesaDTO>> Listar();
        Task<MesaDTO> ObtenerPorId(int id);
        Task Actualizar(ActualizarMesaDTO dto);
    }
}