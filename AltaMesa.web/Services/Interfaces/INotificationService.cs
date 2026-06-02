namespace AltaMesa.web.Services.Interfaces
{
    public interface INotificationService
    {
        void NotificarNuevoPedido(int pedidoId, int mesaNumero);
        void NotificarActualizarPedido(int pedidoId);
        void NotificarNuevoDetalleCocina(int detalleId, int mesaNumero);
        void NotificarActualizarCocina(int detalleId);
        void NotificarProductoListo(int pedidoId, int detalleId, string nombreProducto, int cantidad);
        void NotificarCambioEstadoDetalle(int pedidoId, int detalleId, string nuevoEstado);
        void NotificarProductoEntregado(int detalleId);
        void NotificarPedidoCerrado(int pedidoId);
    }
}
