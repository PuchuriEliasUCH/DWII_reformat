using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

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

        public async Task AgregarDetalle(AgregarDetalleDTO dto)
        {
            await _pedidoRepository.AgregarDetalle(dto).ConfigureAwait(false);
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

        public async Task<List<CocinaDTO>> ListarColaCocina()
        {
            return await _pedidoRepository.ListarColaCocina().ConfigureAwait(false);
        }

        public async Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId, string observacion = null)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var estadoAnterior = await _pedidoRepository.ObtenerEstadoDetalle(detalleId).ConfigureAwait(false);
                await _pedidoRepository.CambiarEstadoDetalle(detalleId, nuevoEstado).ConfigureAwait(false);
                await _pedidoRepository.RegistrarAuditoria(detalleId, estadoAnterior, nuevoEstado, usuarioId, observacion).ConfigureAwait(false);
                scope.Complete();
            }
        }
    }
}
