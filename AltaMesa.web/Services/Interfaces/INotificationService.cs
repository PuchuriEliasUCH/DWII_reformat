namespace AltaMesa.web.Services.Interfaces
{
    public interface INotificationService
    {
        void NotificarNuevoPedido(int pedidoId, int mesaNumero);
        void NotificarActualizarPedido(int pedidoId);
        void NotificarNuevoDetalleCocina(int detalleId, int mesaNumero);
        void NotificarActualizarCocina(int detalleId);
        void NotificarProductoListo(int pedidoId);
        void NotificarPedidoCerrado(int pedidoId);
    }
}