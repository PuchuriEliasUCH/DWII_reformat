using AltaMesa.web.Hubs;
using AltaMesa.web.Services.Interfaces;
using Microsoft.AspNet.SignalR;

namespace AltaMesa.web.Services
{
    public class NotificationService : INotificationService
    {
        public void NotificarNuevoPedido(int pedidoId, int mesaNumero)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.Group("pedidos").nuevoPedido(pedidoId, mesaNumero);
            hubContext.Clients.Group("cocina").nuevoPedido(pedidoId, mesaNumero);
        }

        public void NotificarActualizarPedido(int pedidoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.Group("pedidos").actualizarPedido(pedidoId);
        }

        public void NotificarNuevoDetalleCocina(int detalleId, int mesaNumero)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CocinaHub>();
            hubContext.Clients.Group("cocina").nuevoDetalleCocina(detalleId, mesaNumero);
            hubContext.Clients.Group("pedidos").nuevoDetalleCocina(detalleId, mesaNumero);
        }

        public void NotificarActualizarCocina(int detalleId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CocinaHub>();
            hubContext.Clients.Group("cocina").actualizarDetalleCocina(detalleId);
        }

        public void NotificarProductoListo(int pedidoId, int detalleId, string nombreProducto, int cantidad)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.Group("pedidos").productoListo(detalleId, nombreProducto, cantidad);
            hubContext.Clients.Group("cocina").actualizarCocina();
        }

        public void NotificarProductoEntregado(int detalleId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CocinaHub>();
            hubContext.Clients.Group("cocina").productoEntregado(detalleId);
        }

        public void NotificarCambioEstadoDetalle(int pedidoId, int detalleId, string nuevoEstado)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.Group("pedidos").detalleEstadoCambiado(pedidoId, detalleId, nuevoEstado);
        }

        public void NotificarPedidoCerrado(int pedidoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.Group("pedidos").pedidoCerrado(pedidoId);
            hubContext.Clients.Group("cocina").pedidoCerrado(pedidoId);
        }
    }
}
