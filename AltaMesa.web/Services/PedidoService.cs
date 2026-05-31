using System.Collections.Generic;
using System.Linq;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories;

namespace AltaMesa.web.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoService()
        {
            _pedidoRepository = new PedidoRepository();
        }

        public int CrearPedido(CrearPedidoDTO dto)
        {
            return _pedidoRepository.Crear(dto);
        }

        public void AgregarDetalle(AgregarDetalleDTO dto)
        {
            _pedidoRepository.AgregarDetalle(dto);
        }

        public void AgregarAdicional(AgregarDetalleDTO dto)
        {
            _pedidoRepository.AgregarAdicional(dto);
        }

        public void CerrarPedido(int pedidoId)
        {
            _pedidoRepository.Cerrar(pedidoId);
        }

        public List<PedidoDTO> ListarActivos()
        {
            return _pedidoRepository.ListarActivos();
        }

        public PedidoDTO ObtenerPedido(int id)
        {
            return _pedidoRepository.ObtenerPorId(id);
        }

        public List<DetallePedidoDTO> ObtenerDetalles(int pedidoId)
        {
            return _pedidoRepository.ObtenerDetalles(pedidoId);
        }

        public List<CocinaDTO> ListarColaCocina()
        {
            return _pedidoRepository.ListarColaCocina();
        }

        public void CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId, string observacion = null)
        {
            var estadoAnterior = _pedidoRepository.ObtenerEstadoDetalle(detalleId);
            _pedidoRepository.CambiarEstadoDetalle(detalleId, nuevoEstado);
            _pedidoRepository.RegistrarAuditoria(detalleId, estadoAnterior, nuevoEstado, usuarioId, observacion);
        }
    }
}
