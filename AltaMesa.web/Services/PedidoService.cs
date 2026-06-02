using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<int> CrearPedido(CrearPedidoDTO dto)
        {
            return await _pedidoRepository.Crear(dto).ConfigureAwait(false);
        }

        public async Task<int> AgregarDetalle(AgregarDetalleDTO dto, int usuarioId)
        {
            var (idDetalle, requierePrep) = await _pedidoRepository.AgregarDetalle(dto).ConfigureAwait(false);

            if (!requierePrep)
            {
                await _pedidoRepository.AvanzarSinPreparacion(idDetalle, usuarioId).ConfigureAwait(false);
            }

            return idDetalle;
        }

        public async Task AgregarAdicional(AgregarDetalleDTO dto)
        {
            await _pedidoRepository.AgregarAdicional(dto).ConfigureAwait(false);
        }

        public async Task CerrarPedido(int pedidoId)
        {
            await _pedidoRepository.Cerrar(pedidoId).ConfigureAwait(false);
        }

        public async Task<List<PedidoDTO>> ListarActivos()
        {
            return await _pedidoRepository.ListarActivos().ConfigureAwait(false);
        }

        public async Task<PedidoDTO> ObtenerPedido(int id)
        {
            return await _pedidoRepository.ObtenerPorId(id).ConfigureAwait(false);
        }

        public async Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId)
        {
            return await _pedidoRepository.ObtenerDetalles(pedidoId).ConfigureAwait(false);
        }

        public async Task<DashboardStatsDTO> ObtenerOrdenesDelDia()
        {
            return await _pedidoRepository.ObtenerOrdenesDelDia().ConfigureAwait(false);
        }

        public async Task<List<VentaDiariaDTO>> ObtenerVentasSemana()
        {
            return await _pedidoRepository.ObtenerVentasSemana().ConfigureAwait(false);
        }

        public async Task<List<ProductoMasVendidoDTO>> ObtenerProductosMasVendidos(int top = 5)
        {
            return await _pedidoRepository.ObtenerProductosMasVendidos(top).ConfigureAwait(false);
        }

        public async Task<List<PedidoDTO>> ListarCerrados()
        {
            return await _pedidoRepository.ListarCerrados().ConfigureAwait(false);
        }

        public async Task<List<CocinaDTO>> ListarColaCocina()
        {
            return await _pedidoRepository.ListarColaCocina().ConfigureAwait(false);
        }

        public async Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId)
        {
            await _pedidoRepository.CambiarEstadoDetalle(detalleId, nuevoEstado, usuarioId).ConfigureAwait(false);
        }

        public async Task<List<DetallePedidoDTO>> ObtenerItemsListosMesero(int pedidoId)
        {
            return await _pedidoRepository.ObtenerItemsListosMesero(pedidoId).ConfigureAwait(false);
        }
    }
}
