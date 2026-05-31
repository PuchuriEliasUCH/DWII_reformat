using Microsoft.AspNet.SignalR;
using AltaMesa.web.Hubs;

namespace AltaMesa.web.Services
{
    public class NotificationService
    {
        public void NotificarNuevoPedido(int pedidoId, int mesaNumero)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.All.nuevoPedido(pedidoId, mesaNumero);
        }

        public void NotificarActualizarPedido(int pedidoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.All.actualizarPedido(pedidoId);
        }

        public void NotificarNuevoDetalleCocina(int detalleId, int mesaNumero)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CocinaHub>();
            hubContext.Clients.All.nuevoDetalleCocina(detalleId, mesaNumero);
        }

        public void NotificarActualizarCocina(int detalleId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<CocinaHub>();
            hubContext.Clients.All.actualizarDetalleCocina(detalleId);
        }

        public void NotificarProductoListo(int pedidoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.All.productoListo(pedidoId);
        }

        public void NotificarPedidoCerrado(int pedidoId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PedidoHub>();
            hubContext.Clients.All.pedidoCerrado(pedidoId);
        }
    }
}
