using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<int> CrearPedido(CrearPedidoDTO dto);
        Task<int> AgregarDetalle(AgregarDetalleDTO dto, int usuarioId);
        Task AgregarAdicional(AgregarDetalleDTO dto);
        Task CerrarPedido(int pedidoId);
        Task<List<PedidoDTO>> ListarActivos();
        Task<PedidoDTO> ObtenerPedido(int id);
        Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId);
        Task<List<CocinaDTO>> ListarColaCocina();
        Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId);
        Task<List<DetallePedidoDTO>> ObtenerItemsListosMesero(int pedidoId);
        Task<DashboardStatsDTO> ObtenerOrdenesDelDia();
        Task<List<VentaDiariaDTO>> ObtenerVentasSemana();
        Task<List<ProductoMasVendidoDTO>> ObtenerProductosMasVendidos(int top = 5);
        Task<List<PedidoDTO>> ListarCerrados();
    }
}
