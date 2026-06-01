using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<int> CrearPedido(CrearPedidoDTO dto);
        Task AgregarDetalle(AgregarDetalleDTO dto);
        Task AgregarAdicional(AgregarDetalleDTO dto);
        Task CerrarPedido(int pedidoId);
        Task<List<PedidoDTO>> ListarActivos();
        Task<PedidoDTO> ObtenerPedido(int id);
        Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId);
        Task<List<CocinaDTO>> ListarColaCocina();
        Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId, string observacion = null);
    }
}