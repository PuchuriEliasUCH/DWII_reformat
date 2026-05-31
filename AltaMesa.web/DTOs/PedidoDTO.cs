using System;
using System.Collections.Generic;

namespace AltaMesa.web.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public int IdMesa { get; set; }
        public int NumeroMesa { get; set; }
        public int IdMesero { get; set; }
        public string NombreMesero { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string Estado { get; set; }
        public string ObservacionGeneral { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public List<DetallePedidoDTO> Detalles { get; set; }
    }

    public class DetallePedidoDTO
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string Observacion { get; set; }
        public string EstadoDetalle { get; set; }
        public bool EsAdicional { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class CrearPedidoDTO
    {
        public int Mesa { get; set; }
        public int Mesero { get; set; }
        public string Obs { get; set; }
    }

    public class AgregarDetalleDTO
    {
        public int Pedido { get; set; }
        public int Producto { get; set; }
        public int Cantidad { get; set; }
        public string Obs { get; set; }
    }

    public class CocinaDTO
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public int NumeroMesa { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
        public string EstadoDetalle { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
