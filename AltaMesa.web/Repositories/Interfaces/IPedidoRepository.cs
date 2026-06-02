using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<int> Crear(CrearPedidoDTO dto);
        Task<(int idDetalle, bool requierePreparacion)> AgregarDetalle(AgregarDetalleDTO dto);
        Task AgregarAdicional(AgregarDetalleDTO dto);
        Task Cerrar(int pedidoId);
        Task<string> ObtenerEstadoDetalle(int detalleId);
        Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId);
        Task AvanzarSinPreparacion(int detalleId, int usuarioId);
        Task<List<DetallePedidoDTO>> ObtenerItemsListosMesero(int pedidoId);
        Task<List<PedidoDTO>> ListarActivos();
        Task<PedidoDTO> ObtenerPorId(int id);
        Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId);
        Task<List<CocinaDTO>> ListarColaCocina();
        Task<DashboardStatsDTO> ObtenerOrdenesDelDia();
        Task<List<VentaDiariaDTO>> ObtenerVentasSemana();
        Task<List<ProductoMasVendidoDTO>> ObtenerProductosMasVendidos(int top = 5);
        Task<List<PedidoDTO>> ListarCerrados();
    }
}
