using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task Crear(CrearMesaDTO dto);
        Task<List<MesaDTO>> Listar();
        Task Actualizar(ActualizarMesaDTO dto);
    }
}