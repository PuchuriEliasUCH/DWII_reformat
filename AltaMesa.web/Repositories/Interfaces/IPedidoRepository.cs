using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<int> Crear(CrearPedidoDTO dto);
        Task AgregarDetalle(AgregarDetalleDTO dto);
        Task AgregarAdicional(AgregarDetalleDTO dto);
        Task Cerrar(int pedidoId);
        Task<string> ObtenerEstadoDetalle(int detalleId);
        Task CambiarEstadoDetalle(int detalleId, string estado);
        Task RegistrarAuditoria(int detalleId, string anterior, string nuevo, int usuarioId, string obs);
        Task<List<PedidoDTO>> ListarActivos();
        Task<PedidoDTO> ObtenerPorId(int id);
        Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId);
        Task<List<CocinaDTO>> ListarColaCocina();
    }
}